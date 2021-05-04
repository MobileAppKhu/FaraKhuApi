using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class PollQuestionConfiguration : IEntityTypeConfiguration<PollQuestion>
    {
        public void Configure(EntityTypeBuilder<PollQuestion> builder)
        {
            builder.HasKey(question => question.QuestionId);
            builder.Property(question => question.QuestionId).ValueGeneratedOnAdd();
            builder.Property(question => question.MultiVote).HasDefaultValue(false);
            builder.Property(question => question.IsOpen).HasDefaultValue(true);
            
            // Option 2
            builder.HasOne(question => question.Course)
                .WithMany(course => course.Polls)
                .HasForeignKey(question => question.CourseId);
            // end of Option 2
        }
    }
}