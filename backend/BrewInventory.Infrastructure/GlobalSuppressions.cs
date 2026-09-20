// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Design",
    "MA0048:File name must match type name",
    Justification = "Database migrations",
    Scope = "namespaceanddescendants",
    Target = "~N:BrewInventory.Infrastructure.Persistence.Migrations")]
[assembly: SuppressMessage(
    "Design",
    "MA0051:Method is too long",
    Justification = "Database migrations",
    Scope = "namespaceanddescendants",
    Target = "~N:BrewInventory.Infrastructure.Persistence.Migrations")]
[assembly: SuppressMessage(
    "Design",
    "CA1062:Validate arguments of public methods",
    Justification = "Database migrations",
    Scope = "namespaceanddescendants",
    Target = "~N:BrewInventory.Infrastructure.Persistence.Migrations")
]
