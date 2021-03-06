// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal
{
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public abstract class ConventionAnnotatable : Annotatable, IConventionAnnotatable
    {
        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public new virtual IEnumerable<ConventionAnnotation> GetAnnotations() => base.GetAnnotations().Cast<ConventionAnnotation>();

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual ConventionAnnotation AddAnnotation(
            [NotNull] string name, [CanBeNull] object value, ConfigurationSource configurationSource)
            => (ConventionAnnotation)base.AddAnnotation(name, CreateAnnotation(name, value, configurationSource));

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual ConventionAnnotation SetAnnotation(
            [NotNull] string name, [CanBeNull] object value, ConfigurationSource configurationSource)
        {
            var oldAnnotation = FindAnnotation(name);
            if (oldAnnotation != null)
            {
                if (Equals(oldAnnotation.Value, value))
                {
                    oldAnnotation.UpdateConfigurationSource(configurationSource);
                    return oldAnnotation;
                }

                configurationSource = configurationSource.Max(oldAnnotation.GetConfigurationSource());
            }

            return (ConventionAnnotation)base.SetAnnotation(name, CreateAnnotation(name, value, configurationSource), oldAnnotation);
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public new virtual ConventionAnnotation FindAnnotation(string name)
            => (ConventionAnnotation)base.FindAnnotation(name);

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public new virtual ConventionAnnotation RemoveAnnotation(string name)
            => (ConventionAnnotation)base.RemoveAnnotation(name);

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        protected override Annotation CreateAnnotation(string name, object value)
            => CreateAnnotation(name, value, ConfigurationSource.Explicit);

        private static ConventionAnnotation CreateAnnotation(
            string name, object value, ConfigurationSource configurationSource)
            => new ConventionAnnotation(name, value, configurationSource);

        IEnumerable<IConventionAnnotation> IConventionAnnotatable.GetAnnotations() => GetAnnotations();

        void IConventionAnnotatable.SetAnnotation(string name, object value, bool fromDataAnnotation)
            => SetAnnotation(name, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);

        IConventionAnnotation IConventionAnnotatable.AddAnnotation(string name, object value, bool fromDataAnnotation)
            => AddAnnotation(name, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);

        IConventionAnnotation IConventionAnnotatable.FindAnnotation(string name) => FindAnnotation(name);

        IConventionAnnotation IConventionAnnotatable.RemoveAnnotation(string name) => RemoveAnnotation(name);
    }
}
