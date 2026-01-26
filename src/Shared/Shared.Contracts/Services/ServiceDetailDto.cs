namespace Shared.Contracts.Services;

public record ServiceDetailDto(
    Guid ServiceId,
    string Name,
    string Slug,
    string? Tagline,
    string? Overview,
    string? IconUrl,
    List<string> WhatWeDo,
    List<string> HowWeWork,
    List<string> Benefits,
    List<PricingModelDto> PricingModels,
    List<EngagementTypeDto> EngagementTypes,
    List<TechnologyDto> Technologies,
    List<FaqDto> Faqs
);

public record PricingModelDto(
    string Name,
    string Description
);

public record EngagementTypeDto(
    string Name,
    string Description
);

public record TechnologyDto(
    Guid TechnologyId,
    string Name,
    string? Icon,
    string? Category
);

public record FaqDto(
    string Question,
    string Answer,
    int DisplayOrder
);
