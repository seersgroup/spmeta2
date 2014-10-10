﻿using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WikiPageModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(WikiPageDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;

            var targetPage = FindWikiPageItem(folder, wikiPageModel);

            ModuleFileModelHandler.WithSafeFileOperation(folderModelHost.CurrentLibrary, folder,
                targetPage.Url,
                GetWikiPageName(wikiPageModel),
                Encoding.UTF8.GetBytes(PublishingPageTemplates.RedirectionPageMarkup),
                false,
                null,
                afterFile =>
                {
                    using (var webPartManager = targetPage.File.GetLimitedWebPartManager(PersonalizationScope.Shared))
                    {
                        var webpartPageHost = new WebpartPageModelHost
                        {
                            PageListItem = targetPage,
                            SPLimitedWebPartManager = webPartManager
                        };

                        action(webpartPageHost);

                        targetPage.Update();
                    }
                });
        }

        protected string GetWikiPageName(WikiPageDefinition wikiPageModel)
        {
            var pageName = wikiPageModel.FileName;
            if (!pageName.EndsWith(".aspx")) pageName += ".aspx";

            return pageName;
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;

            //if (!string.IsNullOrEmpty(wikiPageModel.FolderUrl))
            //    throw new Exception("FolderUrl property is not supported yet!");

            var pageItem = FindWikiPageItem(folder, wikiPageModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = pageItem == null ? null : pageItem.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            if (pageItem == null)
            {
                var newWikiPageUrl = GetSafeWikiPageUrl(folder, wikiPageModel);
                var newpage = folder.Files.Add(newWikiPageUrl, SPTemplateFileType.WikiPage);

                newpage.ListItemAllFields[SPBuiltInFieldId.WikiField] = wikiPageModel.Content ?? string.Empty;
                

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newpage,
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });

                newpage.ListItemAllFields.Update();
                newpage.Update();
            }
            else
            {
                pageItem[SPBuiltInFieldId.WikiField] = wikiPageModel.Content ?? string.Empty;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = pageItem.File,
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });

                pageItem.Update();
                pageItem.File.Update();
            }
        }

        protected string GetSafeWikiPageUrl(SPFolder folder, WikiPageDefinition wikiPageModel)
        {
            // TODO, fix +/ to SPUtility
            return folder.ServerRelativeUrl + "/" + GetSafeWikiPageFileName(wikiPageModel);
        }

        protected string GetSafeWikiPageFileName(WikiPageDefinition wikiPageModel)
        {
            var wikiPageName = wikiPageModel.FileName;

            if (!wikiPageName.EndsWith(".aspx"))
                wikiPageName += ".aspx";

            return wikiPageName;
        }

        protected SPListItem FindWikiPageItem(SPFolder folder, WikiPageDefinition wikiPageModel)
        {
            var file = folder.ParentWeb.GetFile(GetSafeWikiPageUrl(folder, wikiPageModel));

            return file.Exists ? file.Item : null;
        }

        #endregion
    }
}
