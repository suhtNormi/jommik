using GymREST.Models.Classes;
using Microsoft.EntityFrameworkCore;
using GymREST.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GymREST.Models.Enums;

namespace GymREST.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Plan> Plans { get; set; }
        public DbSet<InitialData> InitialDatas { get; set; }
        public DbSet<Exercise> Exercises { get; set;}
        public DbSet<PlanExerciseRel> PlanExerciseRels { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<UserChosenPlan>? UserChosenPlans { get; set; }
        public DbSet<Profile>? Profiles { get; set; }
        public DbSet<UserExercise> UserExercises { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<WorkoutEvent>? WorkoutEvents{ get; set; }
        
        public DbSet<CalendarExercise>? CalendarExercises { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InitialData>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Plan>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Exercise>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Plan>().Property(p => p.Id).HasIdentityOptions(startValue: 4);
            modelBuilder.Entity<Exercise>().Property(p => p.Id).HasIdentityOptions(startValue: 6);
            modelBuilder.Entity<PlanExerciseRel>().Property(p => p.Id).HasIdentityOptions(startValue: 3);
            modelBuilder.Entity<PlanExerciseRel>().HasKey(e => new { e.ExerciseId, e.PlanId }); 
            modelBuilder.Entity<Event>().Property(x => x.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<PlanExerciseRel>()
                .HasOne(e => e.Plan)
                .WithMany(s => s.PlanExerciseRels)
                .HasForeignKey(e => e.PlanId);

            modelBuilder.Entity<PlanExerciseRel>()
                .HasOne(e => e.Exercise)
                .WithMany(c => c.PlanExerciseRels)
                .HasForeignKey(e => e.ExerciseId);

            modelBuilder.Entity<UserChosenPlan>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Profile>()
            .HasKey(x => x.Id);

            modelBuilder.Entity<WorkoutEvent>()
            .HasKey(x => x.Id);

            modelBuilder.Entity<WorkoutEvent>()
            .HasKey(x => x.Id);


            modelBuilder.Entity<CalendarExercise>()
            .HasKey(x => x.Id);

                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = 1,
                        Username = "test1",
                        Password = "St9tpNN2zrinRGNUgKWCy4JjZRFEorSQ0Zg3a/8m7k4=", //test1
                       
                    },
                    new User
                    {
                        Id = 2,
                        Username = "test2",
                        Password = "zWoe4T9h2Hj9G4dyUtWwcKwV6zMR1Q0yr3Uch+xSze8=", // test2
                        
                    },

                    new User
                    {
                        Id = 3,
                        Username = "test3",
                        Password = "6RwNz8ehCp0yZ0KkUE7i+Shy+2l7C1Eh9dT/RULwZN8=", // test3
                      
                    }
                );

                modelBuilder.Entity<UserChosenPlan>().HasData(
                    new UserChosenPlan{
                        Id = 1,
                        UserId = 1,
                        PlanId = 1,
                    },
                    new UserChosenPlan{
                        Id = 2,
                        UserId = 2,
                        PlanId = 5,
                    },
                    new UserChosenPlan{
                        Id = 3,
                        UserId = 3,
                        PlanId = 6,
                    }
                );
                // modelBuilder.Entity<InitialData>().HasData(
                //     new InitialData{
                //         Id = 1,
                //         UserId = 1,
                //         Name = "kalle",
                //         Age = 15,
                //         Gender = "Male",
                //         Height = 190,
                //         Weight = 90,
                //         Goal = TrainingGoal.Painduvus,
                //         Frequency = 4,
                //         Level = TrainingLevel.Kesktase
                //     }
                // );

            
modelBuilder.Entity<Exercise>().HasData(
    new Exercise { Id = 1, Name = "Kükid", Sets = 3, Reps = 10, Bodypart = "Legs" },
    new Exercise { Id = 2, Name = "Surumine pingil", Sets = 3, Reps = 10, Bodypart = "Upperbody" },
    new Exercise { Id = 3, Name = "Lainete tõstmine (dumbbell)", Sets = 3, Reps = 10, Bodypart = "Arms" },
    new Exercise { Id = 4, Name = "Tõuked", Sets = 3, Reps = 10, Bodypart = "Upperbody" },
    new Exercise { Id = 5, Name = "Plank", Sets = 3, Reps = 30, Bodypart = "Core" }, // seconds
    new Exercise { Id = 6, Name = "Sumo kükid", Sets = 3, Reps = 10, Bodypart = "Legs" },
    new Exercise { Id = 7, Name = "Säärte sirutused (masin)", Sets = 3, Reps = 10, Bodypart = "Legs" },
    new Exercise { Id = 8, Name = "Säärte painutused (masin)", Sets = 3, Reps = 10, Bodypart = "Legs" },
    new Exercise { Id = 9, Name = "Sild (glute bridge)", Sets = 3, Reps = 15, Bodypart = "Legs" },
    new Exercise { Id = 10, Name = "Push-ups", Sets = 3, Reps = 10, Bodypart = "Upperbody" },
    new Exercise { Id = 11, Name = "Dumbbell shoulder press", Sets = 3, Reps = 10, Bodypart = "Shoulders" },
    new Exercise { Id = 12, Name = "Dumbbell rows", Sets = 3, Reps = 10, Bodypart = "Back" },
    new Exercise { Id = 13, Name = "Tricep dips", Sets = 3, Reps = 10, Bodypart = "Upperbody" },
    new Exercise { Id = 14, Name = "Bicep curls", Sets = 3, Reps = 10, Bodypart = "Arms" },
    new Exercise { Id = 15, Name = "Põlvetõste", Sets = 1, Reps = 15, Bodypart = "Legs" }, 
    new Exercise { Id = 16, Name = "Kükk ja hüpe", Sets = 3, Reps = 10, Bodypart = "Legs" },

    new Exercise { Id = 17, Name = "Pull-ups", Sets = 3, Reps = 8, Bodypart = "Back" },
    new Exercise { Id = 18, Name = "Russian twists", Sets = 3, Reps = 20, Bodypart = "Core" },
    new Exercise { Id = 19, Name = "Mountain climbers", Sets = 3, Reps = 30, Bodypart = "Core" }, 
    new Exercise { Id = 20, Name = "Deadlift", Sets = 3, Reps = 8, Bodypart = "Legs" },
    new Exercise { Id = 21, Name = "Burpees", Sets = 3, Reps = 10, Bodypart = "Fullbody" },
    new Exercise { Id = 22, Name = "Side lunges", Sets = 3, Reps = 10, Bodypart = "Legs" },
    new Exercise { Id = 23, Name = "Plank shoulder taps", Sets = 3, Reps = 20, Bodypart = "Core" },
    new Exercise { Id = 24, Name = "Dumbbell lateral raises", Sets = 3, Reps = 12, Bodypart = "Shoulders" },
    new Exercise { Id = 25, Name = "Renegade rows", Sets = 3, Reps = 10, Bodypart = "Back" },
    new Exercise { Id = 26, Name = "Hanging leg raises", Sets = 3, Reps = 15, Bodypart = "Core" },
    new Exercise { Id = 27, Name = "Farmer's carry", Sets = 3, Reps = 30, Bodypart = "Fullbody" }, 
    new Exercise { Id = 28, Name = "Dumbbell chest fly", Sets = 3, Reps = 12, Bodypart = "Upperbody" },
    new Exercise { Id = 29, Name = "Calf raises", Sets = 3, Reps = 15, Bodypart = "Legs" },
    new Exercise { Id = 30, Name = "Kettlebell swings", Sets = 3, Reps = 12, Bodypart = "Fullbody" }
);



// Treeningplaanid (Plans)
modelBuilder.Entity<Plan>().HasData(
    new Plan
    {
        Id = 1,
        Level = "Algaja",
        Title = "Algaja lihasmassi kasvatamine",
        Description = "See treeningkava keskendub peamiselt lihasmassi suurendamisele, kasutades lihtsaid harjutusi, et aidata algajal alustada tõhusat treeningut.",
        Goal = "Lihasmassi kasvatamine"
    },
    new Plan
    {
        Id = 2,
        Level = "Kesktase",
        Title = "Keskmise taseme lihasmassi kasvatamine",
        Description = "See kava suurendab treeningu intensiivsust ja keerukust, et saavutada lihasmassi kiiremat kasvu.",
        Goal = "Lihasmassi kasvatamine"
    },
    new Plan
    {
        Id = 3,
        Level = "Edasijõudnud",
        Title = "Edasijõudnud lihasmassi kasvatamine",
        Description = "Suure intensiivsusega treeningkava, mis on mõeldud edasijõudnud sportlastele lihasmassi märkimisväärseks suurendamiseks.",
        Goal = "Lihasmassi kasvatamine"
    },
    new Plan
    {
        Id = 4,
        Level = "Algaja",
        Title = "Algaja ülakeha lihasmassi suurendamine",
        Description = "Kava, mis keskendub ülakeha lihasmassi suurendamisele algaja tasemel, kasutades lihtsaid ja tõhusaid harjutusi.",
        Goal = "Suurenda ülakeha lihasmassi"
    },
    new Plan
    {
        Id = 5,
        Level = "Kesktase",
        Title = "Keskmise taseme ülakeha lihasmassi suurendamine",
        Description = "Tõhus treeningkava, mis suurendab ülakeha lihasmassi, integreerides erinevaid harjutusi ja tehnikaid.",
        Goal = "Suurenda ülakeha lihasmassi"
    },
    new Plan
    {
        Id = 6,
        Level = "Edasijõudnud",
        Title = "Edasijõudnud ülakeha lihasmassi suurendamine",
        Description = "Suure intensiivsusega kava, mis on suunatud edasijõudnutele, et maksimeerida ülakeha lihasmassi ja jõudlust.",
        Goal = "Suurenda ülakeha lihasmassi"
    },
    new Plan
    {
        Id = 7,
        Level = "Algaja",
        Title = "Algaja kaalulangetamine",
        Description = "Treeningkava, mis aitab algajatel kaalu langetada, keskendudes tasakaalustatud harjutustele ja madala intensiivsusega kardio.",
        Goal = "Kaalu langetamine"
    },
    new Plan
    {
        Id = 8,
        Level = "Kesktase",
        Title = "Keskmise taseme kaalulangetamine",
        Description = "Intensiivsem treeningkava, mis aitab kaalu langetada ja parandada üldist füüsilist vormi.",
        Goal = "Kaalu langetamine"
    },
    new Plan
    {
        Id = 9,
        Level = "Edasijõudnud",
        Title = "Edasijõudnud kaalulangetamine",
        Description = "Tõhus treeningkava, mis on suunatud edasijõudnutele, et aidata saavutada maksimaalset kaalulangust ja vastupidavust.",
        Goal = "Kaalu langetamine"
    },
    new Plan
    {
        Id = 10,
        Level = "Algaja",
        Title = "Algaja võhma suurendamine",
        Description = "Kava, mis keskendub algaja tasemel vastupidavuse ja võhma suurendamisele.",
        Goal = "Vastupidavuse suurendamine"
    },
    new Plan
    {
        Id = 11,
        Level = "Kesktase",
        Title = "Keskmise taseme võhma suurendamine",
        Description = "Intensiivsem treeningkava, mis keskendub vastupidavuse ja füüsilise vormi parandamisele.",
        Goal = "Vastupidavuse suurendamine"
    },
    new Plan
    {
        Id = 12,
        Level = "Edasijõudnud",
        Title = "Edasijõudnud võhma suurendamine",
        Description = "Suure intensiivsusega kava, mis on suunatud edasijõudnutele, et maksimeerida vastupidavust ja võhma.",
        Goal = "Vastupidavuse suurendamine"
    },
     new Plan
    {
        Id = 13,
        Level = "Algaja",
        Title = "Lihasmassi kasvatamise alustamine",
        Description = "See kava keskendub lihasmassi kasvatamisele, kasutades lihtsaid, kuid tõhusaid harjutusi nagu kükkide ja surumise pingil kombinatsioon.",
        Goal = "Lihasmassi kasvatamine"
    },
    new Plan
    {
        Id = 14,
        Level = "Kesktase",
        Title = "Lihasmassi arendamine kesktasemel",
        Description = "Intensiivne treeningkava, mis sisaldab harjutusi nagu dumbbell shoulder press ja dumbbell rows lihasmassi kasvatamiseks.",
        Goal = "Lihasmassi kasvatamine"
    },
    new Plan
    {
        Id = 15,
        Level = "Edasijõudnud",
        Title = "Tippvormi lihasmassi kasvatamine",
        Description = "Tõhustatud kava, mis kasutab harjutusi nagu pull-ups ja dumbbell chest fly lihasmassi arendamiseks suure intensiivsusega.",
        Goal = "Lihasmassi kasvatamine"
    },
    new Plan
    {
        Id = 16,
        Level = "Algaja",
        Title = "Vastupidavuse arendamine algaja tasemel",
        Description = "Harjutused, mis aitavad algajatel suurendada vastupidavust, keskendudes plankide ja lunges harjutuste kombineerimisele.",
        Goal = "Vastupidavuse suurendamine"
    },
    new Plan
    {
        Id = 17,
        Level = "Kesktase",
        Title = "Vastupidavuse tõstmine kesktasemel",
        Description = "Harjutused nagu mountain climbers ja running, mis aitavad kesktasemel suurendada südame-veresoonkonna vastupidavust.",
        Goal = "Vastupidavuse suurendamine"
    },
    new Plan
    {
        Id = 18,
        Level = "Edasijõudnud",
        Title = "Maksimaalne vastupidavus edasijõudnutele",
        Description = "Harjutused nagu burpees ja farmer's carry, mis on mõeldud edasijõudnutele, et testida ja suurendada vastupidavust äärmuslikel tasemetel.",
        Goal = "Vastupidavuse suurendamine"
    }
);
// Treeningkava ja harjutuse seosed (PlanExerciseRel)
modelBuilder.Entity<PlanExerciseRel>().HasData(
    // Algaja lihasmassi kasvatamine
    new PlanExerciseRel { Id = 1, PlanId = 1, ExerciseId = 1 },
    new PlanExerciseRel { Id = 2, PlanId = 1, ExerciseId = 2 },
    new PlanExerciseRel { Id = 3, PlanId = 1, ExerciseId = 3 },
    new PlanExerciseRel { Id = 4, PlanId = 1, ExerciseId = 4 },
    new PlanExerciseRel { Id = 5, PlanId = 1, ExerciseId = 5 },

    // Keskmise taseme lihasmassi kasvatamine
    new PlanExerciseRel { Id = 6, PlanId = 2, ExerciseId = 1 },
    new PlanExerciseRel { Id = 7, PlanId = 2, ExerciseId = 2 },
    new PlanExerciseRel { Id = 8, PlanId = 2, ExerciseId = 4 },
    new PlanExerciseRel { Id = 9, PlanId = 2, ExerciseId = 6 },
    new PlanExerciseRel { Id = 10, PlanId = 2, ExerciseId = 7 },
    new PlanExerciseRel { Id = 46, PlanId = 2, ExerciseId = 17},


    // Edasijõudnud lihasmassi kasvatamine
    new PlanExerciseRel { Id = 11, PlanId = 3, ExerciseId = 1 },
    new PlanExerciseRel { Id = 12, PlanId = 3, ExerciseId = 2 },
    new PlanExerciseRel { Id = 13, PlanId = 3, ExerciseId = 7 },
    new PlanExerciseRel { Id = 14, PlanId = 3, ExerciseId = 8 },
    new PlanExerciseRel { Id = 15, PlanId = 3, ExerciseId = 9 },
     new PlanExerciseRel { Id = 47, PlanId = 3, ExerciseId = 17},


    // Algaja ülakeha lihasmassi suurendamine
    new PlanExerciseRel { Id = 16, PlanId = 4, ExerciseId = 10 },
    new PlanExerciseRel { Id = 17, PlanId = 4, ExerciseId = 11 },
    new PlanExerciseRel { Id = 18, PlanId = 4, ExerciseId = 12 },
    new PlanExerciseRel { Id = 55, PlanId = 4, ExerciseId = 13 },



    // Keskmise taseme ülakeha lihasmassi suurendamine
    new PlanExerciseRel { Id = 19, PlanId = 5, ExerciseId = 10 },
    new PlanExerciseRel { Id = 20, PlanId = 5, ExerciseId = 11 },
    new PlanExerciseRel { Id = 21, PlanId = 5, ExerciseId = 13 },
    new PlanExerciseRel { Id = 48, PlanId = 5, ExerciseId = 17},

    
    // Edasijõudnud ülakeha lihasmassi suurendamine
    new PlanExerciseRel { Id = 22, PlanId = 6, ExerciseId = 10 },
    new PlanExerciseRel { Id = 23, PlanId = 6, ExerciseId = 11 },
    new PlanExerciseRel { Id = 24, PlanId = 6, ExerciseId = 12 },
    new PlanExerciseRel { Id = 25, PlanId = 6, ExerciseId = 14 },
    new PlanExerciseRel { Id = 49, PlanId = 6, ExerciseId = 17},


     // Algaja kaalulangetamine
    new PlanExerciseRel { Id = 26, PlanId = 7, ExerciseId = 5 },
    new PlanExerciseRel { Id = 27, PlanId = 7, ExerciseId = 15 },
    new PlanExerciseRel { Id = 28, PlanId = 7, ExerciseId = 3 },
    new PlanExerciseRel { Id = 50, PlanId= 7, ExerciseId =  18 },

    // Keskmise taseme kaalulangetamine
    new PlanExerciseRel { Id = 29, PlanId = 8, ExerciseId = 5 },
    new PlanExerciseRel { Id = 30, PlanId = 8, ExerciseId = 15 },
    new PlanExerciseRel { Id = 31, PlanId = 8, ExerciseId = 4 },
    new PlanExerciseRel { Id = 32, PlanId = 8, ExerciseId = 6 },
        new PlanExerciseRel { Id = 51, PlanId= 8, ExerciseId =  18 },


    // Edasijõudnud kaalulangetamine
    new PlanExerciseRel { Id = 33, PlanId = 9, ExerciseId = 5 },
    new PlanExerciseRel { Id = 34, PlanId = 9, ExerciseId = 15 },
    new PlanExerciseRel { Id = 35, PlanId = 9, ExerciseId = 1 },
    new PlanExerciseRel { Id = 36, PlanId = 9, ExerciseId = 2 },
    new PlanExerciseRel { Id = 52, PlanId= 9, ExerciseId =  18 },

    // Algaja võhma suurendamine
    new PlanExerciseRel { Id = 37, PlanId = 10, ExerciseId = 5 },
    new PlanExerciseRel { Id = 38, PlanId = 10, ExerciseId = 15 },
    new PlanExerciseRel { Id = 39, PlanId = 10, ExerciseId = 6 },
    new PlanExerciseRel { Id = 54, PlanId = 10, ExerciseId = 19},

    // Keskmise taseme võhma suurendamine
    new PlanExerciseRel { Id = 40, PlanId = 11, ExerciseId = 5 },
    new PlanExerciseRel { Id = 41, PlanId = 11, ExerciseId = 15 },
    new PlanExerciseRel { Id = 42, PlanId = 11, ExerciseId = 8 },
        new PlanExerciseRel { Id = 53, PlanId = 11, ExerciseId = 19 },


    // Edasijõudnud võhma suurendamine
    new PlanExerciseRel { Id = 43, PlanId = 12, ExerciseId = 5 },
    new PlanExerciseRel { Id = 44, PlanId = 12, ExerciseId = 15 },
    new PlanExerciseRel { Id = 45, PlanId = 12, ExerciseId = 19 },
    new PlanExerciseRel { Id = 56, PlanId = 12, ExerciseId = 21},
    new PlanExerciseRel { Id = 57, PlanId = 12, ExerciseId = 7 }, 
    new PlanExerciseRel { Id = 58, PlanId = 12, ExerciseId = 10 }, // Jooks
    new PlanExerciseRel { Id = 59, PlanId = 12, ExerciseId = 16 }, // Kükk ja hüpe

    // 13. Keskmise taseme kaalulangetamine
    new PlanExerciseRel { Id = 60, PlanId = 13, ExerciseId = 1 }, // Kükid
    new PlanExerciseRel { Id = 61, PlanId = 13, ExerciseId = 5 }, // Plank
    new PlanExerciseRel { Id = 62, PlanId = 13, ExerciseId = 15 }, // Jooks
    new PlanExerciseRel { Id = 63, PlanId = 13, ExerciseId = 16 }, // Kükk ja hüpe
    new PlanExerciseRel { Id = 64, PlanId = 13, ExerciseId = 7 }, // Säärte sirutused (masin)

    // 14. Edasijõudnud kaalulangetamine
    new PlanExerciseRel { Id = 65, PlanId = 14, ExerciseId = 1 }, // Kükid
    new PlanExerciseRel { Id = 66, PlanId = 14, ExerciseId = 5 }, // Plank
    new PlanExerciseRel { Id = 67, PlanId = 14, ExerciseId = 15 }, // Jooks
    new PlanExerciseRel { Id = 68, PlanId = 14, ExerciseId = 16 }, // Kükk ja hüpe
    new PlanExerciseRel { Id = 69, PlanId = 14, ExerciseId = 8 }, // Säärte painutused (masin)

    // 15. Algaja võhma suurendamine
    new PlanExerciseRel { Id = 70, PlanId = 15, ExerciseId = 1 }, // Kükid
    new PlanExerciseRel { Id = 71, PlanId = 15, ExerciseId = 2 }, // Surumine pingil
    new PlanExerciseRel { Id = 72, PlanId = 15, ExerciseId = 5 }, // Plank
    new PlanExerciseRel { Id = 73, PlanId = 15, ExerciseId = 6 }, // Sumo kükid

    // 16. Keskmise taseme võhma suurendamine
    new PlanExerciseRel { Id = 74, PlanId = 16, ExerciseId = 1 }, // Kükid
    new PlanExerciseRel { Id = 75, PlanId = 16, ExerciseId = 2 }, // Surumine pingil
    new PlanExerciseRel { Id = 76, PlanId = 16, ExerciseId = 6 }, // Sumo kükid
    new PlanExerciseRel { Id = 77, PlanId = 16, ExerciseId = 7 }, // Säärte sirutused (masin)
    new PlanExerciseRel { Id = 78, PlanId = 16, ExerciseId = 15 }, // Jooks

    // 17. Edasijõudnud võhma suurendamine
    new PlanExerciseRel { Id = 79, PlanId = 17, ExerciseId = 1 }, // Kükid
    new PlanExerciseRel { Id = 80, PlanId = 17, ExerciseId = 2 }, // Surumine pingil
    new PlanExerciseRel { Id = 81, PlanId = 17, ExerciseId = 6 }, // Sumo kükid
    new PlanExerciseRel { Id = 82, PlanId = 17, ExerciseId = 8 }, // Säärte painutused (masin)
    new PlanExerciseRel { Id = 83, PlanId = 17, ExerciseId = 15 }, // Jooks

    // 18. Üldine füüsiline vorm
    new PlanExerciseRel { Id = 84, PlanId = 18, ExerciseId = 1 }, // Kükid
    new PlanExerciseRel { Id = 85, PlanId = 18, ExerciseId = 2 }, // Surumine pingil
    new PlanExerciseRel { Id = 86, PlanId = 18, ExerciseId = 3 }, // Lainete tõstmine (dumbbell)
    new PlanExerciseRel { Id = 87, PlanId = 18, ExerciseId = 5 }, // Plank
    new PlanExerciseRel { Id = 88, PlanId = 18, ExerciseId = 16 }  // Kükk ja hüpe

);
modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Jooga",
                    Description = "Kestab tund aega",
                    Start = "2024-12-17 14:00",
                    End = "2024-12-17 15:00",
                    Class = "training",
                    Content = "⇢ Lisainfo ⇠"
                },
                new Event
                {
                    Id = 2,
                    Title = "HIIT Workout",
                    Description = "Kõrge intensiivsusega treening",
                    Start = "2024-12-18 10:00",
                    End = "2024-12-18 11:00",
                    Class = "training",
                    Content = "⇢ Lisainfo ⇠"
                },
                new Event
                {
                    Id = 3,
                    Title = "Õppimine",
                    Description = "Matemaatika KT jaoks kordamine",
                    Start = "2024-12-19 09:00",
                    End = "2024-12-19 12:00",
                    Class = "event",
                    Content = "⇢ Lisainfo ⇠"
                }
            );
        }
    }
}
        