﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class SecurityGroupDefinitionValidator : DefinitionBaseValidator
    {
        public override void Validate(DefinitionBase definition, List<ValidationResult> result)
        {
            Validate<SecurityGroupDefinition>(definition, model => model
                .NotEmptyString(m => m.Name, result)
                .NoSpacesBeforeOrAfter(m => m.Name, result));
        }
    }
}
