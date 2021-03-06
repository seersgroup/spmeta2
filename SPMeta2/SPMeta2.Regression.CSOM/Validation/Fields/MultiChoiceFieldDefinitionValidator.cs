﻿using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using System;
using System.Linq;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class MultiChoiceFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(MultiChoiceFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var textField = spObject.Context.CastTo<FieldMultiChoice>(spObject);
            var textDefinition = model.WithAssertAndCast<MultiChoiceFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            textFieldAssert.ShouldBeEqual(m => m.FillInChoice, o => o.FillInChoice);

            if (textDefinition.Choices.Count > 0)
            {
                var hasChoices = true;

                foreach (var dstChoice in textField.Choices)
                {
                    if (textDefinition.Choices.FirstOrDefault(c => c.ToUpper() == dstChoice.ToUpper()) == null)
                    {
                        hasChoices = false;
                    }
                }

                textFieldAssert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.Choices);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = hasChoices == true
                    };
                });
            }
            else
            {
                textFieldAssert.SkipProperty(m => m.Choices, "Choices.Count == 0. Skipping.");
            }
        }
    }
}
