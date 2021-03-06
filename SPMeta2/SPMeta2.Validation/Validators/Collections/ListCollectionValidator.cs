﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Collections
{
    public class ListCollectionValidator : CollectionValidatorBase
    {
        public override void Validate(IEnumerable<DefinitionBase> models, List<ValidationResult> result)
        {
            Validate<ListDefinition>(models, model =>
            {
                CheckIfUnique(model, m => m.Title, result);
                CheckIfUnique(model, m => m.Url, result);
            });
        }
    }
}
