using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace GymREST.Data.Repos
{
    public class CalendarExerciseRepo(DataContext context)
    {
        private readonly DataContext context = context;

        public async Task<CalendarExercise> CreateCalendarExercise(CalendarExercise calendarExercise){

            context.CalendarExercises.Add(calendarExercise);
            await context.SaveChangesAsync();
            return calendarExercise;
        }

        public async Task<CalendarExercise> UpdateCalendarExercise(CalendarExercise updateExercise){
            context.CalendarExercises.Update(updateExercise);
            await context.SaveChangesAsync();
            return updateExercise;
        }
        public async Task<CalendarExercise> GetCalendarExerciseById(int id ){
            return await context.CalendarExercises.FindAsync(id);
        }

        public async Task<List<CalendarExercise>> GetAllCalendarExercises(){
            return await context.CalendarExercises.ToListAsync();
        }

        public async Task<List<CalendarExercise>> GetExercisesByName(string name){
            return await context.CalendarExercises
            .Where(x => x.Name == name)
            .ToListAsync();
        }
        
        // public async Task<List<object>> GetExerciseBodypartData(int userId, string from, string to){
        //     var exercises = await context.CalendarExercises
        //     .Where(x => x.UserId == userId && x.DateAdded >= from && x.DateAdded <= to)
        //     .ToListAsync();

        //     if(exercises == null){
        //         throw new Exception("No exercises found for the given user ID.");
        //     }

        //     var groupedExercises = exercises
        //     .GroupBy(x => x.Bodypart)
        //     .Select(group => new {
        //         Bodypart = group.Key,
        //         TotalSets = group.Sum(exercise => exercise.Sets),
        //         TotalReps = group.Sum(exercise => exercise.Reps)
        //     })
        //     .ToList();
        //     return groupedExercises.Select(g => (object)g).ToList();
        // }
        public async Task<List<object>> GetExerciseBodypartData(int userId, string from, string to)
        {
            
            if (!DateTime.TryParseExact(from, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
            {
                throw new ArgumentException("Invalid 'from' date format. Use 'yyyy-MM-dd HH:mm'.");
            }

            if (!DateTime.TryParseExact(to, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
            {
                throw new ArgumentException("Invalid 'to' date format. Use 'yyyy-MM-dd HH:mm'.");
            }

            
            var exercises = await context.CalendarExercises
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var filteredExercises = exercises
                .Where(x =>
                {
                    if (DateTime.TryParseExact(x.DateAdded, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateAdded))
                    {
                        return dateAdded >= fromDate && dateAdded <= toDate;
                    }
                    return false;
                })
                .ToList();

            if (!filteredExercises.Any())
            {
                throw new Exception("No exercises found for the given user ID and date range.");
            }

            var groupedExercises = filteredExercises
                .GroupBy(x => x.Bodypart)
                .Select(group => new
                {
                    Bodypart = group.Key,
                    TotalSets = group.Sum(exercise => exercise.Sets),
                    TotalReps = group.Sum(exercise => exercise.Reps)
                })
                .ToList();

            return groupedExercises.Select(g => (object)g).ToList();
        }
        

        
    }
}