﻿using System;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeLinkModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeLinkDefinition); }
        }

        protected SPContentType GetListContentType(SPList list, ContentTypeLinkDefinition definition)
        {
            SPContentType result = null;

            if (!string.IsNullOrEmpty(definition.ContentTypeName))
                result = list.ContentTypes[definition.ContentTypeName];

            if (result == null && !string.IsNullOrEmpty(definition.ContentTypeId))
            {
                var linkContenType = new SPContentTypeId(definition.ContentTypeId);
                var bestMatch = list.ContentTypes.BestMatch(linkContenType);

                if (bestMatch.IsChildOf(linkContenType))
                    result = list.ContentTypes[bestMatch];
            }

            return result;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            if (!list.ContentTypesEnabled)
            {
                TraceService.ErrorFormat((int)LogEventId.ModelProvisionCoreCall,
                    "List [{0}] does not allow content types. Throwing SPMeta2Exception.",
                    list.RootFolder.ServerRelativeUrl);

                throw new SPMeta2Exception(string.Format("List [{0}] does not allow content types.",
                    list.RootFolder.ServerRelativeUrl));
            }

            var web = list.ParentWeb;

            var contentTypeId = new SPContentTypeId(contentTypeLinkModel.ContentTypeId);
            var targetContentType = web.AvailableContentTypes[contentTypeId];

            if (targetContentType == null)
            {
                TraceService.ErrorFormat((int)LogEventId.ModelProvisionCoreCall,
                    "Cannot find site content type by ID: [{0}]. Throwing SPMeta2Exception.",
                    contentTypeId);

                throw new SPMeta2Exception(string.Format("Cannot find site content type with ID [{0}].", contentTypeId));
            }

            var currentListContentType = GetListContentType(list, contentTypeLinkModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentListContentType,
                ObjectType = typeof(SPContentType),
                ObjectDefinition = contentTypeLinkModel,
                ModelHost = modelHost
            });

            if (currentListContentType == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list content type link");

                var listCt = list.ContentTypes.Add(targetContentType);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = listCt,
                    ObjectType = typeof(SPContentType),
                    ObjectDefinition = contentTypeLinkModel,
                    ModelHost = modelHost
                });

                //list.Update();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list content type link");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentListContentType,
                    ObjectType = typeof(SPContentType),
                    ObjectDefinition = contentTypeLinkModel,
                    ModelHost = modelHost
                });

            }
        }

        #endregion
    }
}
