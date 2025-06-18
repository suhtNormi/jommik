using GymREST.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace GymREST.Data.Repos
{
    public class ExercisesRepo(DataContext context)
    {
        private readonly DataContext _context = context;

        public async Task<List<Exercise>> GetAllExercises(){
            IQueryable<Exercise> query = _context.Exercises.AsQueryable();
            return await query.ToListAsync();
        }

        // Save a new UserExercise to the database
        public async Task<UserExercise> SaveUserExerciseToDb(UserExercise userExercise)
        {
            _context.UserExercises.Add(userExercise);
            await _context.SaveChangesAsync();
            return userExercise;
        }

        // Get all UserExercises
        public async Task<List<UserExercise>> GetAllUserExercises()
        {
            return await _context.UserExercises.ToListAsync();
        }

        // Check if a UserExercise exists in the database
        public async Task<bool> UserExerciseExistsInDb(int id)
        {
            return await _context.UserExercises.AnyAsync(x => x.Id == id);
        }

        // Get a specific UserExercise by Id
        public async Task<UserExercise> GetUserExerciseById(int id)
        {
            return await _context.UserExercises.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Exercise>> GetBodypartExercise(string bodypart){
            return await _context.Exercises
            .Where(x => x.Bodypart == bodypart)
            .ToListAsync();
        }
    }
}
