<script setup lang="ts">
import { ref, watch } from 'vue';

const planStore = usePlanStore(); 


const userWorkoutDetails = ref<{ [exerciseId: string]: { sets: Array<{ reps: number; notes: string; DateTime: string }> } }>({});
const showModal = ref(false); // controls modal visibility
const savedWorkoutDetails = ref<{ [exerciseId: string]: { name: string; sets: Array<{ reps: number; notes: string }> } }>({});

watch(
  () => planStore.selectedPlan,  
  (newPlan) => {
    if (newPlan) {
      userWorkoutDetails.value = Object.fromEntries(
        newPlan.exercises.map((exercise: any) => [
          exercise.id,
          {
            sets: Array.from({ length: exercise.sets }, () => ({
              reps: 0,
              notes: '',
              DateTime: new Date().toISOString() 
            })),
          },
        ])
      );
    }
  },
  { immediate: true }
);

const saveWorkoutDetails = async () => {
  try {
    const savedExercises: Record<string, { name: string; sets: Array<{ reps: number; notes: string }> }> = {};    
    for (const [exerciseId, details] of Object.entries(userWorkoutDetails.value)) {
      const sets = details.sets.map((set) => ({
        reps: set.reps,
        notes: set.notes,
      }));
      for (const set of details.sets) {
        const payload = {
          userId: 1, // Example user ID, replace with the actual logged-in user's ID if necessary
          exerciseId: parseInt(exerciseId), // Convert the exerciseId to a number
          reps: set.reps,
          sets: 3, // Assuming one row per set (if you need to pass more details, adjust this)
          notes: set.notes,
          DateTime: set.DateTime, 
        };

        const response = await fetch('http://localhost:5034/api/UserExercise', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload),
        });

        if (!response.ok) {
          throw new Error(`Failed to save exercise with ID: ${exerciseId}`);
        }
      }
      savedExercises[exerciseId] = {
        name: planStore.selectedPlan?.exercises.find((e: any) => e.id == exerciseId)?.name || 'Unknown Exercise',
        sets: sets,
      };
    }
    savedWorkoutDetails.value = savedExercises;
    showModal.value = true;
    alert('Workout details saved successfully!');
  } catch (error) {
    console.error('Error saving workout details:', error);
    alert('An error occurred while saving workout details.');
  }
};
</script>


<template>
  <div class="flex justify-center items-center h-full bg-gray-100 dark:bg-gray-900">
    <div class="w-full max-w-3xl p-6 bg-white dark:bg-gray-800 rounded-lg shadow-xl">

      <header class="mb-8">
        <h2 class="text-3xl font-extrabold text-gray-900 dark:text-gray-100 leading-tight">
          {{ planStore.selectedPlan?.title || 'Valitud plaan puudub' }}
        </h2>
        <p class="text-md text-gray-600 dark:text-gray-400 mt-2">
          {{ planStore.selectedPlan?.description }}
        </p>
      </header>

      <main class="overflow-y-auto bg-gray-50 dark:bg-gray-700 rounded-lg shadow p-4 max-h-[70vh] space-y-6">
        <div v-if="planStore.selectedPlan && planStore.selectedPlan.exercises.length" class="space-y-4">
          <h3 class="text-xl font-semibold text-gray-800 dark:text-gray-200">Harjutused:</h3>

          <div
            v-for="exercise in planStore.selectedPlan.exercises"
            :key="exercise.id"
            class="p-4 border border-gray-300 rounded-lg bg-gray-100 dark:bg-gray-600 hover:bg-gray-200 dark:hover:bg-gray-500 transition duration-300"
          >
            <h4 class="text-lg font-semibold text-gray-900 dark:text-gray-100">{{ exercise.name }}</h4>
            <p class="text-sm text-gray-600 dark:text-gray-400">
              Planeeritud: {{ exercise.sets }} seeriad ja {{ exercise.reps }} kordust
            </p>

            <div class="mt-4 space-y-3">
              <div v-for="(set, index) in userWorkoutDetails[exercise.id]?.sets" :key="index" class="space-y-2">
                <div class="flex items-center space-x-3">
                  <span class="text-sm text-gray-700 dark:text-gray-300">Seeria {{ index + 1 }}:</span>
                  <input
                    type="number"
                    v-model.number="userWorkoutDetails[exercise.id].sets[index].reps"
                    class="w-20 p-2 text-sm text-gray-700 border border-gray-300 rounded-md dark:bg-gray-700 dark:text-gray-100 focus:ring-2 focus:ring-purple-500"
                    placeholder="Reps"
                  />
                  <input
                    type="text"
                    v-model="userWorkoutDetails[exercise.id].sets[index].notes"
                    class="w-3/4 p-2 text-sm text-gray-700 border border-gray-300 rounded-md dark:bg-gray-700 dark:text-gray-100 focus:ring-2 focus:ring-purple-500"
                    placeholder="Märkmed"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>

        <p v-else class="text-gray-500 dark:text-gray-400">Harjutusi ei leitud.</p>
      </main>

      <div v-if="showModal" class="fixed inset-0 bg-gray-500 bg-opacity-50 flex items-center justify-center z-50">
        <div class="bg-white p-6 rounded-lg shadow-xl max-w-lg w-full">
          <h3 class="text-xl font-semibold text-gray-900">Salvesta Treening</h3>
          
          <div v-for="(exercise, exerciseId) in savedWorkoutDetails" :key="exerciseId" class="mt-4">
            <h4 class="text-lg font-semibold text-gray-800">{{ exercise.name }}</h4>
            <div v-for="(set, index) in exercise.sets" :key="index" class="mt-2">
              <p>Seeria {{ index + 1 }}: {{ set.reps }} kordust - Märkmed: {{ set.notes }}</p>
            </div>
          </div>
          <button @click="showModal = false" class="mt-4 px-4 py-2 bg-purple-600 text-white rounded-md">
            Close
          </button>
        </div>
      </div>

      <footer class="mt-6">
        <button
          @click="saveWorkoutDetails"
          class="w-full py-3 bg-purple-600 text-white rounded-md hover:bg-purple-700 focus:outline-none focus:ring-2 focus:ring-purple-400 transition duration-300"
          v-if="planStore.selectedPlan && planStore.selectedPlan.exercises.length"
        >
          Salvestsa harjutuse andmed
        </button>
      </footer>
    </div>
  </div>
</template>

<style scoped>
  @import '../designs/workout.css';
  </style>