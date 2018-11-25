namespace NetCoreTemplate.DAL.Initializers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NetCoreTemplate.DAL.Extensions;
    using NetCoreTemplate.DAL.Initializers.Translation;
    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.DAL.Models.Translation;
    using NetCoreTemplate.SharedKernel.Extensions;

    using Microsoft.EntityFrameworkCore;

    using TrackableEntities.Common.Core;

    public static class TranslationInitializer
    {
        public static TEntity SeedEntityTranslations<TEntity, TProperty>(
            DatabaseContext context,
            Expression<Func<DatabaseContext, DbSet<TEntity>>> expression,
            Expression<Func<TEntity, TProperty>> keyExpression,
            TEntity entity)
            where TEntity : EntityTranslation
        {
            var entitySet = expression.Compile()(context);
            var value = keyExpression.Compile()(entity);
            var p = keyExpression.Parameters.First();
            var predicate = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(keyExpression.Body, Expression.Constant(value)), p);
            var exists = entitySet.Any(predicate);
            var listLanguages = context.Language.ToList();

            if (exists)
            {
                var databaseEntity = entitySet.First(predicate);

                if (databaseEntity.EntityLabelDefinition_Id.IsNullOrDefault() || databaseEntity.EntityLabelDefinition_Id == default(int))
                {
                    AddNewEntityTranslation(context, listLanguages, entity, databaseEntity);
                    return databaseEntity;
                }

                databaseEntity.Translation = entity.Translation;
                UpdateTranslations(context, listLanguages, databaseEntity);
                return databaseEntity;
            }

            AddNewEntityTranslation(context, listLanguages, entity, null);
            return entity;
        }

        private static void UpdateTranslations<TEntity>(
            DatabaseContext context,
            List<Language> listLanguages,
            TEntity entity)
            where TEntity : EntityTranslation
        {
            var entityLabelDefinition = context.EntityLabelDefinition
                        .FirstOrDefault(x => x.Id == entity.EntityLabelDefinition_Id);

            var translations = context.EntityLabel
                .Where(x => x.EntityLabelDefinition_Id == entityLabelDefinition.Id)
                .ToList();

            if (!string.IsNullOrWhiteSpace(entity.Translation.NL))
            {
                var language = listLanguages.First(x => x.Id == 1);

                EntityLabel label;

                if (translations.Any(x => x.Language_Id == language.Id))
                {
                    label = translations.First(x => x.Language_Id == language.Id);
                    label.Label = entity.Translation.NL;
                    label.TrackingState = TrackingState.Modified;

                    context.EntityLabel.Update(label);
                }
                else
                {
                    label = new EntityLabel
                    {
                        EntityLabelDefinition_Id = entityLabelDefinition.Id,
                        EntityLabelDefinition = entityLabelDefinition,
                        Language = language,
                        Language_Id = language.Id,
                        Label = entity.Translation.NL,
                        TrackingState = TrackingState.Added
                    };

                    context.EntityLabel.Add(label);
                }
            }

            if (!string.IsNullOrWhiteSpace(entity.Translation.EN))
            {
                var language = listLanguages.First(x => x.Id == 2);

                EntityLabel label;

                if (translations.Any(x => x.Language_Id == language.Id))
                {
                    label = translations.First(x => x.Language_Id == language.Id);
                    label.Label = entity.Translation.EN;
                    label.TrackingState = TrackingState.Modified;

                    context.EntityLabel.Update(label);
                }
                else
                {
                    label = new EntityLabel
                    {
                        EntityLabelDefinition_Id = entityLabelDefinition.Id,
                        EntityLabelDefinition = entityLabelDefinition,
                        Language = language,
                        Language_Id = language.Id,
                        Label = entity.Translation.EN,
                        TrackingState = TrackingState.Added
                    };

                    context.EntityLabel.Add(label);
                }
            }

            if (!string.IsNullOrWhiteSpace(entity.Translation.DE))
            {
                var language = listLanguages.First(x => x.Id == 3);

                EntityLabel label;

                if (translations.Any(x => x.Language_Id == language.Id))
                {
                    label = translations.First(x => x.Language_Id == language.Id);
                    label.Label = entity.Translation.DE;
                    label.TrackingState = TrackingState.Modified;

                    context.EntityLabel.Update(label);
                }
                else
                {
                    label = new EntityLabel
                    {
                        EntityLabelDefinition_Id = entityLabelDefinition.Id,
                        EntityLabelDefinition = entityLabelDefinition,
                        Language = language,
                        Language_Id = language.Id,
                        Label = entity.Translation.DE,
                        TrackingState = TrackingState.Added
                    };

                    context.EntityLabel.Add(label);
                }
            }

            if (!string.IsNullOrWhiteSpace(entity.Translation.FR))
            {
                var language = listLanguages.First(x => x.Id == 4);

                EntityLabel label;

                if (translations.Any(x => x.Language_Id == language.Id))
                {
                    label = translations.First(x => x.Language_Id == language.Id);
                    label.Label = entity.Translation.FR;
                    label.TrackingState = TrackingState.Modified;

                    context.EntityLabel.Update(label);
                }
                else
                {
                    label = new EntityLabel
                    {
                        EntityLabelDefinition_Id = entityLabelDefinition.Id,
                        EntityLabelDefinition = entityLabelDefinition,
                        Language = language,
                        Language_Id = language.Id,
                        Label = entity.Translation.FR,
                        TrackingState = TrackingState.Added
                    };

                    context.EntityLabel.Add(label);
                }
            }
        }

        private static void AddNewEntityTranslation<TEntity>(
            DatabaseContext context,
            List<Language> listLanguages,
            TEntity entity,
            TEntity databaseEntity)
            where TEntity : EntityTranslation
        {
            var entityLabelDefinition = new EntityLabelDefinition
            {
                Key = $"{typeof(TEntity).Name.ToLower()}.{StringExtension.GenerateRandomString()}",
                TrackingState = TrackingState.Added
            };

            context.EntityLabelDefinition.Add(entityLabelDefinition);

            if (databaseEntity.IsNullOrDefault())
            {
                entity.EntityLabelDefinition = entityLabelDefinition;
                entity.EntityLabelDefinition_Id = entityLabelDefinition.Id;
            }
            else
            {
                databaseEntity.EntityLabelDefinition = entityLabelDefinition;
                databaseEntity.EntityLabelDefinition_Id = entityLabelDefinition.Id;
            }

            if (!string.IsNullOrWhiteSpace(entity.Translation.NL))
            {
                var language = listLanguages.First(x => x.Id == 1);
                var label = new EntityLabel
                {
                    EntityLabelDefinition_Id = entityLabelDefinition.Id,
                    EntityLabelDefinition = entityLabelDefinition,
                    Language = language,
                    Language_Id = language.Id,
                    Label = entity.Translation.NL,
                    TrackingState = TrackingState.Added
                };

                context.EntityLabel.Add(label);
            }

            if (!string.IsNullOrWhiteSpace(entity.Translation.EN))
            {
                var language = listLanguages.First(x => x.Id == 2);
                var label = new EntityLabel
                {
                    EntityLabelDefinition_Id = entityLabelDefinition.Id,
                    EntityLabelDefinition = entityLabelDefinition,
                    Language = language,
                    Language_Id = language.Id,
                    Label = entity.Translation.EN,
                    TrackingState = TrackingState.Added
                };

                context.EntityLabel.Add(label);
            }

            if (!string.IsNullOrWhiteSpace(entity.Translation.DE))
            {
                var language = listLanguages.First(x => x.Id == 3);
                var label = new EntityLabel
                {
                    EntityLabelDefinition_Id = entityLabelDefinition.Id,
                    EntityLabelDefinition = entityLabelDefinition,
                    Language = language,
                    Language_Id = language.Id,
                    Label = entity.Translation.DE,
                    TrackingState = TrackingState.Added
                };

                context.EntityLabel.Add(label);
            }

            if (!string.IsNullOrWhiteSpace(entity.Translation.FR))
            {
                var language = listLanguages.First(x => x.Id == 4);
                var label = new EntityLabel
                {
                    EntityLabelDefinition_Id = entityLabelDefinition.Id,
                    EntityLabelDefinition = entityLabelDefinition,
                    Language = language,
                    Language_Id = language.Id,
                    Label = entity.Translation.FR,
                    TrackingState = TrackingState.Added
                };

                context.EntityLabel.Add(label);
            }
        }

        public static void SeedTranslations(
            DatabaseContext context,
            List<BaseTranslationInitializer> listTranslationInitializers)
        {
            var listTranslationLabelDefinitions = context
                .TranslationLabelDefinition
                .ToList();

            var listLanguages = context.Language.ToList();

            var seedList = new List<TranslationLabel>();

            foreach (var initializer in listTranslationInitializers)
            {
                var translations = initializer.Translations();

                foreach (var translation in translations)
                {
                    var definition = listTranslationLabelDefinitions.FirstOrDefault(
                        x => x.Module.Equals(initializer.Module, StringComparison.OrdinalIgnoreCase) &&
                             x.Type.Equals(initializer.Type, StringComparison.OrdinalIgnoreCase) &&
                             x.Key.Equals(translation.Key, StringComparison.OrdinalIgnoreCase));

                    var translationLabels = new List<TranslationLabel>();

                    if (definition.IsNullOrDefault())
                    {
                        definition = new TranslationLabelDefinition
                        {
                            Module = initializer.Module,
                            Type = initializer.Type,
                            Key = translation.Key
                        };

                        context.TranslationLabelDefinition.Add(definition);
                        context.SaveChanges();
                    }
                    else
                    {
                        var databaseTranslationLabels = context
                            .TranslationLabel
                            .Where(x => x.TranslationLabelDefinition_Id == definition.Id)
                            .ToList();

                        translationLabels.AddRange(databaseTranslationLabels);
                    }

                    if (!string.IsNullOrWhiteSpace(translation.NL))
                    {
                        var language = listLanguages.First(x => x.Id == 1);
                        var translationExists = translationLabels.Any(
                            x => x.Language_Id == language.Id &&
                                 x.TranslationLabelDefinition_Id == definition.Id);

                        var translationLabel = new TranslationLabel
                        {
                            TranslationLabelDefinition_Id = definition.Id,
                            TranslationLabelDefinition = definition,
                            Language_Id = language.Id,
                            Language = language,
                            Label = translation.NL,
                            TrackingState = translationExists ? TrackingState.Unchanged : TrackingState.Added
                        };

                        seedList.Add(translationLabel);
                    }

                    if (!string.IsNullOrWhiteSpace(translation.EN))
                    {
                        var language = listLanguages.First(x => x.Id == 2);
                        var translationExists = translationLabels.Any(
                            x => x.Language_Id == language.Id &&
                                 x.TranslationLabelDefinition_Id == definition.Id);

                        var translationLabel = new TranslationLabel
                        {
                            TranslationLabelDefinition_Id = definition.Id,
                            TranslationLabelDefinition = definition,
                            Language_Id = language.Id,
                            Language = language,
                            Label = translation.EN,
                            TrackingState = translationExists ? TrackingState.Unchanged : TrackingState.Added
                        };

                        seedList.Add(translationLabel);
                    }

                    if (!string.IsNullOrWhiteSpace(translation.DE))
                    {
                        var language = listLanguages.First(x => x.Id == 3);
                        var translationExists = translationLabels.Any(
                            x => x.Language_Id == language.Id &&
                                 x.TranslationLabelDefinition_Id == definition.Id);

                        var translationLabel = new TranslationLabel
                        {
                            TranslationLabelDefinition_Id = definition.Id,
                            TranslationLabelDefinition = definition,
                            Language_Id = language.Id,
                            Language = language,
                            Label = translation.DE,
                            TrackingState = translationExists ? TrackingState.Unchanged : TrackingState.Added
                        };

                        seedList.Add(translationLabel);
                    }

                    if (!string.IsNullOrWhiteSpace(translation.FR))
                    {
                        var language = listLanguages.First(x => x.Id == 4);
                        var translationExists = translationLabels.Any(
                            x => x.Language_Id == language.Id &&
                                 x.TranslationLabelDefinition_Id == definition.Id);

                        var translationLabel = new TranslationLabel
                        {
                            TranslationLabelDefinition_Id = definition.Id,
                            TranslationLabelDefinition = definition,
                            Language_Id = language.Id,
                            Language = language,
                            Label = translation.FR,
                            TrackingState = translationExists ? TrackingState.Unchanged : TrackingState.Added
                        };

                        seedList.Add(translationLabel);
                    }
                }
            }

            context.AddOrUpdateRange(seedList);
        }
    }
}
