using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GymREST.Data.Repos
{
    public class ProfileRepo(DataContext context)
    {
        private readonly DataContext context = context;



        public async Task<bool> ProfileExistsInDb(int id) => await context.Users.AnyAsync(u => u.Id == id);

        public async Task<Profile> GetLastProfile(){
            var profile =  await context.Profiles
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();
            if (profile == null){
                throw new Exception("No profile found");
            }
            return profile;
        }

        public async Task<Profile> SaveProfileToDb(Profile profile){
            context.Add(profile);
            await context.SaveChangesAsync();
            return profile;
        }

        public async Task<List<Profile>> GetAllProfiles(){
            IQueryable<Profile> query = context.Profiles.AsQueryable();
            return await query.ToListAsync();
        }

        public async Task<InitialData> PullInitialDataFromProfile(int userId){
        var profile = await context.Profiles
        .Include(p => p.InitialData) 
        .FirstOrDefaultAsync(p => p.UserId == userId); 

        if (profile == null || profile.InitialData == null)
        {
            throw new ArgumentException($"InitialData for UserId {userId} does not exist.");
        }

        return profile.InitialData;
        }



        public async Task<Profile> CreateProfile(int userId, InitialData request){
            var user = await context.Users.FindAsync(userId);
            if(user == null){
                throw new ArgumentException($"Profile with Id {userId} does not exist.");
            }

            if (request == null){
                throw new ArgumentNullException(nameof(request), "InitialData cannot be null.");
            }

            var initialData = new InitialData{
                UserId = userId,
                Name = request.Name,
                Age = request.Age,
                Gender = request.Gender,
                Height = request.Height,
                Weight = request.Weight,
                Goal = request.Goal,
                Frequency = request.Frequency,
                Level = request.Level,
            };

            context.InitialDatas.Add(initialData);
            await context.SaveChangesAsync();

            var profile = new Profile{
                UserId = userId,
                InitialDataId = initialData.Id
            };

            context.Profiles.Add(profile);
            await context.SaveChangesAsync();
            return profile;
        }

        
    }
}