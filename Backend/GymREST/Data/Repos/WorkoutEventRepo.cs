using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GymREST.Data.Repos
{
    public class WorkoutEventRepo(DataContext context)
    {
        private readonly DataContext context = context;


        public async Task<WorkoutEvent> CreateEvent(int userId, WorkoutEvent data){
            var exercises = await context.CalendarExercises
            .Where(x => x.UserId == userId)
            .ToListAsync();

            if (exercises == null){
                throw new Exception("No exercises found for the given user ID.");
            }
            data.CalendarExercises = exercises;
            data.UserId = userId;

            context.WorkoutEvents.Add(data);
            await context.SaveChangesAsync();
            return data;
        }

        public async Task<List<WorkoutEvent>> GetAllEvents()
        {
            return await context.WorkoutEvents.ToListAsync();
        }

        public async Task<WorkoutEvent> GetEventById(int id)
        {
            return await context.WorkoutEvents.FindAsync(id);
        }

        public async Task<List<WorkoutEvent>> GetEventByName(string name){
            return await context.WorkoutEvents
            .Where(x => x.Name == name)
            .ToListAsync();
        }


        public async Task<WorkoutEvent> UpdateWorkoutEvent(WorkoutEvent updatedWorkoutEvent)
        {
            context.WorkoutEvents.Update(updatedWorkoutEvent);
            await context.SaveChangesAsync();
            return updatedWorkoutEvent;
        }

        public async Task<bool> DeleteWorkoutEvent(int id)
        {
            var workoutEventToDelete = await context.WorkoutEvents.FindAsync(id);
            if (workoutEventToDelete == null) return false;

            context.WorkoutEvents.Remove(workoutEventToDelete);
            await context.SaveChangesAsync();
            return true;
        }
    }
}