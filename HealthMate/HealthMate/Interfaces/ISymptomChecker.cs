using HealthMate.Models;
using Refit;

namespace HealthMate.Interfaces;
public interface ISymptomChecker
{
    [Get("/symptoms/?birth_year={birthYear}&gender={gender}&body_part={bodyPart}")]
    Task<IEnumerable<Symptoms>> GroupList(int birthYear, string gender, string bodyPart);

}