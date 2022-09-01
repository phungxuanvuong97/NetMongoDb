// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;

namespace Infrastructure.Hosting.Configuration
{
    public interface IValidatableOptions
    {
        IEnumerable<ConfigurationError> Validate();
    }
}
