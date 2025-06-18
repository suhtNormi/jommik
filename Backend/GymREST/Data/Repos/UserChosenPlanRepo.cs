using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GymREST.Data.Repos
{
    public class UserChosenPlanRepo(DataContext context) {
        private readonly DataContext context = context;


//read

        public async Task<bool> UserExistsInDb(int id) => await context.Users.AnyAsync(x => x.Id == id);
        public async Task<List<UserChosenPlan>> GetUserChosenPlans() {
            if (context == null ){
                throw new InvalidOperationException("Ei eksisteeri");
            }
            return await context.UserChosenPlans.ToListAsync();
        }

        public async Task<UserChosenPlan?> GetUserChosenPlanByUserId (int userId){
            return await context.UserChosenPlans.FirstOrDefaultAsync(x => x.UserId == userId);
        }


        public async Task<List<Exercise?>> GetAllUserChosenPlanExercises(int userId){
            return await context.Plans
            .Include(x => x.PlanExerciseRels)
            .ThenInclude(rel => rel.Exercise)
            .Where(user => user.Id == userId)
            .SelectMany(plan => plan.PlanExerciseRels.Select(rel => rel.Exercise))
            .ToListAsync();
        }

        public async Task<string> GetUserChosenPlanName(int userId) {
            var planName = await context.UserChosenPlans
            .Include(x => x.Plan)
            .FirstOrDefaultAsync(x => x.UserId == userId);

            return planName?.Plan?.Title;
        }
        

        //create
        public async Task<UserChosenPlan> CreateUserChosenPlan(int userId, int planId) {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with Id {userId} does not exist.");
            }

            var plan = await context.Plans.FindAsync(planId);
            if (plan == null)
            {
                throw new ArgumentException($"Plan with Id {planId} does not exist.");
            }

            var userChosenPlan = new UserChosenPlan{
                UserId = userId,
                PlanId = planId,
                User = user,
                Plan = plan
            };
            
            context.UserChosenPlans.Add(userChosenPlan);

            await context.SaveChangesAsync();

            return userChosenPlan;
        }

        //change
        public async Task<UserChosenPlan> ChangeUserPlan(int userId, int planId){
            var userChosenPlan = await context.UserChosenPlans.FindAsync(userId);
            if (userChosenPlan == null){
                throw new ArgumentException($"UserChosenPlan with Id {userId} not found.");
            }

            userChosenPlan.PlanId = planId;
            await context.SaveChangesAsync();
            return userChosenPlan;
        }
        
        

    }
}