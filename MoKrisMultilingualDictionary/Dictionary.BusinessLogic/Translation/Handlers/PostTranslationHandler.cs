using Dictionary.BusinessLogic.Translation.Requests;
using Dictionary.Data;
using Dictionary.Domain;
using Dictionary.Domain.Builders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.Translation.Handlers
{
    public class PostTranslationHandler : IRequestHandler<PostTranslationRequest>
    {
        private readonly DictionaryContext dbContext;

        public PostTranslationHandler(DictionaryContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(PostTranslationRequest request, CancellationToken cancellationToken)
        {
            var wordTranslationGroups = await dbContext.WordTranslationGroups
                .Where(wtg => wtg.WordId == request.SourceWordId || wtg.WordId == request.TargetWordId)
                .ToListAsync(cancellationToken);

            var commonTranslationGroupIds = await dbContext.TranslationGroups
                .AsNoTracking()
                .Where(tg => tg.WordTranslationGroups.Any(wtg => wtg.WordId == request.SourceWordId)
                            && tg.WordTranslationGroups.Any(wtg => wtg.WordId == request.TargetWordId))
                .Select(tg => tg.TranslationGroupId)
                .ToListAsync(cancellationToken);

            var newLinkedTranslationGroupIds = request.LinkedTranslationGroupIds
                .Where(ltgId => !commonTranslationGroupIds.Contains(ltgId));

            var newWordTranslationGroups = new List<WordTranslationGroup>();
            foreach (var newLinkedTranslationGroupId in newLinkedTranslationGroupIds)
            {
                AddWordTranslationGroupToListIfNeeded(wordTranslationGroups, newLinkedTranslationGroupId, request.SourceWordId, newWordTranslationGroups);
                AddWordTranslationGroupToListIfNeeded(wordTranslationGroups, newLinkedTranslationGroupId, request.TargetWordId, newWordTranslationGroups);
            }

            if (newWordTranslationGroups.Any())
            {
                await dbContext.WordTranslationGroups.AddRangeAsync(newWordTranslationGroups, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private void AddWordTranslationGroupToListIfNeeded(List<WordTranslationGroup> wordTranslationGroups,
            int newLinkedTranslationGroupId,
            int wordId,
            List<WordTranslationGroup> newWordTranslationGroups)
        {
            if (!wordTranslationGroups.Any(wtg => wtg.WordId == wordId && wtg.TranslationGroupId == newLinkedTranslationGroupId))
            {
                newWordTranslationGroups.Add(new WordTranslationGroupBuilder()
                    .SetTranslationGroupId(newLinkedTranslationGroupId)
                    .SetWordId(wordId)
                    .Build());
            }
        }
    }
}
