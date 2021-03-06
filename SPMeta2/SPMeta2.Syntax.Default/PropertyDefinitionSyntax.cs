﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class PropertyDefinitionSyntax
    {
        public static ModelNode AddProperty(this ModelNode model, PropertyDefinition definition)
        {
            return AddProperty(model, definition, null);
        }

        public static ModelNode AddProperty(this ModelNode model, PropertyDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #region array overload

        public static ModelNode AddProperties(this ModelNode model, IEnumerable<PropertyDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}