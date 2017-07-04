﻿using System;
using Sdl.Web.Common.Models;
using Sdl.Web.Modules.Core.Models;

namespace Sdl.Web.Modules.Test.Models
{
    [SemanticEntity("TSI2316")]
    public class Tsi2316TestEntity : EntityModel
    {
        public KeywordModel NotPublishedKeyword { get; set; }
        public Tsi2316TestKeyword PublishedKeyword { get; set; }
    }

    [SemanticEntity("NavigationTaxonomyKeywordMetadata")]
    public class Tsi2316TestKeyword : KeywordModel
    {
        [SemanticProperty("TextField")]
        public string TextField { get; set; }

        [SemanticProperty("NumberField")]
        public double NumberField { get; set; }

        [SemanticProperty("DateField")]
        public DateTime? DateField { get; set; }

        [SemanticProperty("CompLinkField")]
        public Link CompLinkField { get; set; }

        [SemanticProperty("CompLinkField")]
        public Article LinkedEntity { get; set; }

        [SemanticProperty("KeywordField")]
        public KeywordModel KeywordField { get; set; }
    }
}
