﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Used by regression testing infrastructure to indicate properties which have to be changes with a new provision.
    /// </summary>
    public class ExpectUpdate : Attribute
    {

    }

    public class ExpectUpdateAsLCID : ExpectUpdate
    {

    }

    public class ExpectUpdateAsCamlQuery : ExpectUpdate
    {

    }
    public class ExpectUpdateAsInternalFieldName : ExpectUpdate
    {

    }
    public class ExpectUpdateAsUser : ExpectUpdate
    {

    }

    public class ExpectUpdateAsBasePermission : ExpectUpdate
    {

    }

    public class ExpectUpdateAsUIVersion : ExpectUpdate
    {

    }

    public class ExpectUpdateAsWebPartPageLayoutTemplate : ExpectUpdate
    {

    }
    public class ExpectUpdateAsFileName : ExpectUpdate
    {
        public string Extension { get; set; }
    }

    public class ExpectUpdateAsPageLayoutFileName : ExpectUpdate
    {

    }

    public class ExpectUpdateAsPublishingPageContentType : ExpectUpdate
    {

    }
    public class ExpectUpdateAsUInt : ExpectUpdate
    {

    }

    public class ExpectUpdateAsByte : ExpectUpdate
    {

    }

    public class ExpectUpdateAsIntRange : ExpectUpdate
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }

    public class ExpectUpdateAsUrlFieldFormat : ExpectUpdate
    {
    }

    public class ExpectUpdateAsUrl : ExpectUpdate
    {
        public ExpectUpdateAsUrl()
        {
            Extension = "txt";
        }

        public string Extension { get; set; }
    }

    public class ExpectUpdateAsCalculatedFieldFormula : ExpectUpdate
    {

    }

    public class ExpectUpdateAssCalculatedFieldOutputType : ExpectUpdate
    {

    }

    public class ExpectUpdateAssCalculatedFieldReferences : ExpectUpdate
    {

    }


    public class ExpectUpdateAsDateTimeFieldCalendarType : ExpectUpdate
    {

    }

    public class ExpectUpdateAsDateTimeFieldDisplayFormat : ExpectUpdate
    {

    }

    public class ExpectUpdateAsDateTimeFieldFriendlyDisplayFormat : ExpectUpdate
    {

    }

    public class ExpectUpdateAsFieldUserSelectionMode : ExpectUpdate
    {

    }

    public class ExpectUpdateAsTargetControlType : ExpectUpdate
    {

    }
}
