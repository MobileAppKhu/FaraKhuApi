using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class PollAnswerConfiguration : IEntityTypeConfiguration<PollAnswer>
    {
        public void Configure(EntityTypeBuilder<PollAnswer> builder)
        {
            builder.HasKey(answer => answer.AnswerId);
            builder.Property(answer => answer.AnswerId).ValueGeneratedOnAdd();
            builder.HasOne(answer => answer.Question)
                .WithMany(question => question.Answers)
                .HasForeignKey(answer => answer.QuestionId);
            builder.HasMany(answer => answer.Voters)
                .WithMany(student => student.PollAnswers);
        }
    }
}