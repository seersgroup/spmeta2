﻿using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Identity;

namespace SPMeta2.Definitions
{
    /// <summary>
    ///  Allows to attach field to the target list.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPField", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class ListFieldLinkDefinition : DefinitionBase
    {
        #region properties

        #region properties

        /// <summary>
        /// ID of the target field to be attached to content type.
        /// BuiltInFieldId class can be used to utilize out of the box SharePoint fields. 
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public Guid FieldId { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool AddToDefaultView { get; set; }

        [ExpectValidation]
        [DataMember]
        public BuiltInAddFieldOptions AddFieldOptions { get; set; }

        #endregion

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ListFieldLinkDefinition>(this)
                          .AddPropertyValue(p => p.FieldId)
                          .ToString();
        }

        #endregion
    }
}
