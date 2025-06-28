using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

public class QuizQuestionsService : IQuizQuestionsService
{
    private readonly AppDbContext _context;

    public QuizQuestionsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<QuizQuestionFormVM?> GetQuestionFormAsync(int? questionId, int quizId)
    {
        if (questionId.HasValue)
        {
            var question = await _context.QuizQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == questionId.Value);

            if (question == null) return null;

            return new QuizQuestionFormVM
            {
                QuestionId = question.Id,
                QuizId = question.QuizId,
                QuestionText = question.QuestionText,
                QuestionType = question.QuestionType,
                Options = question.Options.Select(o => new QuizOptionVM
                {
                    OptionId = o.Id,
                    OptionText = o.OptionText,
                    IsCorrect = o.IsCorrect
                }).ToList()
            };
        }
        else
        {
            // Form mới cho câu hỏi mới
            return new QuizQuestionFormVM
            {
                QuizId = quizId,
                Options = new List<QuizOptionVM>
                {
                    new QuizOptionVM(), new QuizOptionVM() // 2 dòng trống mặc định
                }
            };
        }
    }

    public async Task SaveQuestionAsync(QuizQuestionFormVM vm)
    {
        QuizQuestion question;

        if (vm.QuestionId.HasValue)
        {
            // Cập nhật
            question = await _context.QuizQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == vm.QuestionId.Value);

            if (question == null) return;

            question.QuestionText = vm.QuestionText;
            question.QuestionType = vm.QuestionType;

            // Xoá các option cũ
            _context.QuizOptions.RemoveRange(question.Options);

            // Thêm mới options
            question.Options = vm.Options.Select(o => new QuizOption
            {
                OptionText = o.OptionText,
                IsCorrect = o.IsCorrect
            }).ToList();

            _context.QuizQuestions.Update(question);
        }
        else
        {
            // Thêm mới
            question = new QuizQuestion
            {
                QuizId = vm.QuizId,
                QuestionText = vm.QuestionText,
                QuestionType = vm.QuestionType,
                Options = vm.Options.Select(o => new QuizOption
                {
                    OptionText = o.OptionText,
                    IsCorrect = o.IsCorrect
                }).ToList()
            };

            _context.QuizQuestions.Add(question);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteQuestionAsync(int questionId)
    {
        var question = await _context.QuizQuestions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == questionId);

        if (question != null)
        {
            _context.QuizOptions.RemoveRange(question.Options);
            _context.QuizQuestions.Remove(question);
            await _context.SaveChangesAsync();
        }
    }
}
