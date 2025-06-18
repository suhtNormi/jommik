<template>
  <div class="container">
    <h2>{{ title }}</h2>
    <p class="description">{{ description }}</p>

    <div class="exercise-list">
      <div class="exercise-card" v-for="(exercise, index) in exercises" :key="exercise.id">
        <h3>{{ exercise.name }}</h3>
        <p class="planned">Planned: {{ exercise.sets }} sets of {{ exercise.reps }} reps</p>

        <div v-for="(rep, setIndex) in exercise.actualReps" :key="setIndex" class="set-input">
          <label class="set-label">Set {{ setIndex + 1 }}:</label>
          <input
            type="number"
            v-model.number="exercise.actualReps[setIndex]"
            min="0"
            class="reps-input"
            placeholder="Reps"
          />
        </div>
      </div>
    </div>

    
    

    <!-- Button to open the workout naming modal -->
    <button class="save-button" @click="openNameModal">Save Workout</button>
    <p v-if="message" class="message">{{ message }}</p>

    <!-- Modal for naming the workout -->
    <div v-if="showNameModal" class="modal-overlay">
      <div class="modal">
        <h3>Name Your Workout</h3>
        <input type="text" v-model="workoutName" placeholder="Enter workout name" />
        <div class="modal-buttons">
          <button @click="saveWorkout">Save</button>
          <button @click="closeNameModal">Cancel</button>
        </div>
      </div>
    </div>
  </div>
</template>

  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRoute, useRouter } from 'vue-router'; 
  import { useStatStore } from '#build/imports';
  
  // List of exercises 
  const exercises = ref([])
  const message = ref('')
  const title = ref('')
  const description = ref('')
  const showNameModal = ref(false);
  const workoutName = ref('');
  const startTime = ref('');
  const endTime = ref('');
  const route = useRoute();
  const router = useRouter();
  const {createWorkoutEvent, getUserChosenPlanExercises, getPlanData, createCalendarExercise} = useStatStore();
  
  const fetchPlanData = async () => {
    try {
      const data = await getPlanData();
      title.value = data.title;
      description.value = data.description;
    } catch(error){
      console.error('Failed to fetch exercises:', error)
    }
  }

  const fetchExercises = async () => {
    try {
    const data = await getUserChosenPlanExercises()
    exercises.value = data.map(exercise => ({
      ...exercise,
      actualReps: Array(exercise.sets).fill(0), 
      notes: Array(exercise.sets).fill('')      //käib settide kaupa
    }))

    } catch(error){
      console.error('Failed to fetch exercises:', error)
    }
  }

  const setEndTime = () => {
  endTime.value = new Date().toISOString().slice(0, 16).replace('T', ' ');
  console.log('End Time:', endTime.value);
  };

  const openNameModal = () => {
  showNameModal.value = true;
  };

  
  const closeNameModal = () => {
    showNameModal.value = false;
    workoutName.value = '';
  };

  const saveWorkout = async () => {
  try {
    const today = new Date().toISOString().slice(0, 16).replace('T', ' '); // Format: YYYY-MM-DD

    // Filter exercises with valid reps (ignoring empty or zero reps)
    const filledExercises = exercises.value.map(exercise => {
      const filledReps = exercise.actualReps.filter(rep => rep > 0);
      return {
        name: exercise.name,
        sets: filledReps.length,
        reps: filledReps.reduce((acc, val) => acc + val, 0),
        bodypart: exercise.bodypart,
        dateAdded: today,
      };
    }).filter(exercise => exercise.sets > 0); // Keep only exercises with at least one valid set

    if (filledExercises.length === 0) {
      message.value = 'No valid exercises to save.';
      return;
    }

    if (!workoutName.value) {
      message.value = 'Please enter a name for your workout.';
      return;
    }
    setEndTime();
    // Create a WorkoutEvent object
    const workoutEvent = {
      Name: workoutName.value,
      CalendarExercises: filledExercises,
      Start: startTime.value,
      End: endTime.value,
    };

    console.log('Creating Workout Event:', workoutEvent);

    
    const workoutResponse = await createWorkoutEvent(workoutEvent);
    console.log('Workout Event Created:', workoutResponse);

    // salvestab harjutused eraldi kohta
    for (const exercise of filledExercises) {
      await createCalendarExercise(exercise);
    }

      message.value = 'Workout and exercises saved successfully!';
      closeNameModal();
      router.push('/workout-calendar');
    } catch (error) {
      console.error('Failed to save workout:', error);
      message.value = 'Failed to save workout.';
    }
  };



  onMounted(async () => {
    await fetchExercises();
    await fetchPlanData();
    
    startTime.value = route.query.startTime || '';
    console.log('Start Time:', startTime.value);
  });


  </script>
  
  <style scoped>
.container {
  max-width: 600px;
  margin: 2rem auto;
  padding: 2rem;
  background-color: #f9f9f9;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  font-family: Arial, sans-serif;
}

h2 {
  color: #333;
  text-align: center;
  margin-bottom: 1rem;
}

.description {
  text-align: center;
  margin-bottom: 2rem;
  color: #666;
}

.exercise-list {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.exercise-card {
  background: #fff;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.05);
}

.exercise-card h3 {
  margin-bottom: 0.5rem;
  color: #333;
}

.planned {
  color: #777;
  margin-bottom: 1rem;
}

.set-input {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.75rem;
}

.set-label {
  font-weight: bold;
  width: 70px;
}

.reps-input,
.notes-input {
  padding: 0.5rem;
  border: 1px solid #ccc;
  border-radius: 4px;
  width: 100px;
}

.notes-input {
  flex: 1;
}

.save-button {
  background-color: #6a0dad;
  color: #fff;
  padding: 0.75rem;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  width: 100%;
  font-size: 1rem;
  margin-top: 2rem;
}

.save-button:hover {
  background-color: #5a0ca1;
}

.message {
  text-align: center;
  color: #42b883;
  font-weight: bold;
  margin-top: 1rem;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal {
  background: #fff;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
  text-align: center;
}

.modal h3 {
  margin-bottom: 1rem;
}

.modal input {
  width: 100%;
  padding: 0.5rem;
  margin-bottom: 1rem;
  border: 1px solid #ccc;
  border-radius: 5px;
}

.modal-buttons {
  display: flex;
  justify-content: space-between;
}

.modal-buttons button {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.modal-buttons button:first-child {
  background-color: #42b883;
  color: white;
}

.modal-buttons button:last-child {
  background-color: #ccc;
  color: #333;
}

.modal-buttons button:hover {
  opacity: 0.9;
}
</style>
  