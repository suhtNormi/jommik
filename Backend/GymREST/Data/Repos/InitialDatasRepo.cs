using GymREST.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace GymREST.Data.Repos;
public class InitialDatasRepo(DataContext context) {
    private readonly DataContext context = context;
    public async Task<List<InitialData>> GetUserData() {
        if (context == null) {
            throw new InvalidOperationException();
        }
        return await context.InitialDatas.ToListAsync() ?? [];
    }

    //CREATE
    public async Task<InitialData> SaveUserToDb(InitialData initialData) {
        context.Add(initialData);
        await context.SaveChangesAsync();
        return initialData;
    }

    //UPDATE
    public async Task<InitialData> UpdateUserData(int userId,InitialData updatedData) {
        if (updatedData == null)
        {
            throw new ArgumentNullException(nameof(updatedData), "Updated data cannot be null.");
        }
        var existingData = await context.InitialDatas.FirstOrDefaultAsync(x => x.UserId == userId);

        if (existingData == null)
        {
            throw new InvalidOperationException($"No InitialData found for UserId {userId}.");
        }
        existingData.Name = updatedData.Name;
        existingData.Age = updatedData.Age;
        existingData.Gender = updatedData.Gender;
        existingData.Height = updatedData.Height;
        existingData.Weight = updatedData.Weight;
        existingData.Goal = updatedData.Goal;
        existingData.Frequency = updatedData.Frequency;
        existingData.Level = updatedData.Level;

        if (!string.IsNullOrEmpty(updatedData.ProfileImageUrl)) {
        existingData.ProfileImageUrl = updatedData.ProfileImageUrl;
        }

        await context.SaveChangesAsync();

        return existingData;
    }

    //DELETE
     public async Task DeleteUserData() {
        var userData = await context.InitialDatas.ToListAsync(); 
        if (userData != null) {
            context.InitialDatas.RemoveRange(userData);               
            await context.SaveChangesAsync();
        }
    }
}
