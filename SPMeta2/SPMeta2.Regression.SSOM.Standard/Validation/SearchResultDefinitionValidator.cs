﻿using System;
using System.Linq;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class SearchResultDefinitionValidator : SearchConfigurationModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SearchResultDefinition>("model", value => value.RequireNotNull());

            // TODO

            //var assert = ServiceFactory.AssertService
            //               .NewAssert(definition, spObject)
            //                     .ShouldNotBeNull(spObject);
        }

        #endregion
    }
}
