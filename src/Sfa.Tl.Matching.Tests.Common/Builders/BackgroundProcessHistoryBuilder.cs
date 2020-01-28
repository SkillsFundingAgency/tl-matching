using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common.Extensions;

namespace Sfa.Tl.Matching.Tests.Common.Builders
{
    public class BackgroundProcessHistoryBuilder
    {
        private readonly MatchingDbContext _context;

        public IList<BackgroundProcessHistory> BackgroundProcessHistories { get; }

        public BackgroundProcessHistoryBuilder(MatchingDbContext context)
        {
            _context = context;
            BackgroundProcessHistories = new List<BackgroundProcessHistory>();
        }

        public BackgroundProcessHistoryBuilder CreateBackgroundProcessHistories(int numberOfBackgroundProcessHistories = 1, string createdBy = null,
            DateTime? createdOn = null, string modifiedBy = null, DateTime? modifiedOn = null)
        {
            for (var i = 0; i < numberOfBackgroundProcessHistories; i++)
            {
                var backgroundProcessHistoryNumber = i + 1;
                var recordCount = i * 2 + 1;
                var status =
                    backgroundProcessHistoryNumber % 3 == 1 ? BackgroundProcessHistoryStatus.Pending.ToString() :
                    backgroundProcessHistoryNumber % 3 == 2 ? BackgroundProcessHistoryStatus.Processing.ToString() :
                    BackgroundProcessHistoryStatus.Complete.ToString();

                CreateBackgroundProcessHistory(backgroundProcessHistoryNumber,
                    recordCount,
                    $"Process {backgroundProcessHistoryNumber}",
                    status, $"Status Message {backgroundProcessHistoryNumber}",
                    createdBy, createdOn, modifiedBy, modifiedOn);
            }

            return this;
        }

        public BackgroundProcessHistoryBuilder CreateBackgroundProcessHistory(int id,
            int recordCount = 1, string processType = "", string status = "Pending", string statusMessage = "",
            string createdBy = null, DateTime? createdOn = null,
            string modifiedBy = null, DateTime? modifiedOn = null)
        {
            var backgroundProcessHistory = new BackgroundProcessHistory
            {
                Id = id,
                RecordCount = recordCount,
                ProcessType = processType,
                Status = status,
                StatusMessage = statusMessage,
                CreatedBy = createdBy,
                CreatedOn = createdOn ?? default(DateTime),
                ModifiedBy = modifiedBy,
                ModifiedOn = modifiedOn,
            };

            BackgroundProcessHistories.Add(backgroundProcessHistory);

            return this;
        }

        public BackgroundProcessHistoryBuilder ClearData()
        {
            if (!BackgroundProcessHistories.IsNullOrEmpty())
                _context.BackgroundProcessHistory.RemoveRange(BackgroundProcessHistories);

            _context.SaveChanges();

            BackgroundProcessHistories.Clear();

            return this;
        }

        public BackgroundProcessHistoryBuilder SaveData()
        {
            if (!BackgroundProcessHistories.IsNullOrEmpty())
                _context.AddRange(BackgroundProcessHistories);

            _context.SaveChanges();

            return this;
        }
    }
}