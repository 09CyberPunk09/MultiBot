using Common.Entites.Questionaires;
using Persistence.Common.DataAccess.Interfaces;
using System;

namespace Application.Services.Questions;

public class QuestionService : AppService
{
	private readonly IRepository<Question> _questionRepository;
	private readonly IRepository<Answer> _answerRepository;
	public QuestionService(IRepository<Question> repository, IRepository<Answer> answerRepository)
	{
		_questionRepository = repository;
		_answerRepository = answerRepository;
	}

	public Question Get(Guid id)
	{
		return _questionRepository.Get(id);
	}

	public Answer AnswerQuestion(Guid questionaireSessionId, Guid questionId, string text)
	{
		var result = _answerRepository.Add(new()
		{
			Text = text,
			QuestionaireSessionId = questionaireSessionId,
			QuestionId = questionId,
		});
		return result;
	}
}
