using GymREST.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace GymREST.Data.Repos
{
    public class PlansRepo(DataContext context)
    {
        private readonly DataContext context = context;
        
        //CREATE
        public async Task<Plan> SavePlanToDb(Plan plan) {
            context.Add(plan);
            await context.SaveChangesAsync();
            return plan;
        }

        //READ
        public async Task<List<Plan>> GetAllPlans() {
            IQueryable<Plan> query = context.Plans.AsQueryable();
            
            return await query.ToListAsync();
        }
        public async Task<List<Plan>> GetAllPlansByExercise() {
            var contextPlans = context.Plans 
            .Include(s => s.PlanExerciseRels)
            .ThenInclude(s => s.Exercise)
            .ToListAsync();
            return await contextPlans;

        }
        public async Task<List<Exercise?>> GetAllExercisesByPlanId(int planId){
            return await context.Plans
            .Include(plan => plan.PlanExerciseRels)
            .ThenInclude(rel => rel.Exercise)
            .Where(plan => plan.Id == planId)
            .SelectMany(plan => plan.PlanExerciseRels.Select(rel => rel.Exercise))
            .ToListAsync();
        }

        public async Task<List<Plan>> GetPlansByGoalAndLevel(string goal, string level)
        {
        return await context.Plans
        .Include(p => p.PlanExerciseRels)
        .ThenInclude(rel => rel.Exercise)
        .Where(p => p.Goal == goal && p.Level == level) 
        .ToListAsync();
        }
        public async Task<Plan> GetPlanById(int id)
        {
        return await context.Plans
        .Include(p => p.PlanExerciseRels)
        .ThenInclude(rel => rel.Exercise)
        .FirstOrDefaultAsync(p => p.Id == id);
        }
      public async Task<bool> PlanExistsInDb(int id) => await context.Plans.AnyAsync(x => x.Id == id);

        //UPDATE 
        public async Task<bool> UpdatePlan(int id, Plan plan) {

            bool isIdsMatch = id == plan.Id;
            bool planExists = await PlanExistsInDb(id);

            if(!isIdsMatch || !planExists){
                return false;
            }

            context.Update(plan);
            int updatedRecordsCount = await context.SaveChangesAsync();
            return updatedRecordsCount == 1;
        }

        //DELETE
        public async Task<bool> DeletePlanById(int id) {
            Plan? planInDb = await GetPlanById(id);
            if (planInDb is null){
                return false;
            }
            context.Remove(planInDb);
            int changesCount = await context.SaveChangesAsync();

            return changesCount == 1;
        }
        
    }
}