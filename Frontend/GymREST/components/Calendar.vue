<template>
  <div>
    <vue-cal
      class="custom-calendar"
      :events="events"
      editable-events
      events-count-on-year-view
      events-on-month-view="short"
      locale="et"
      :disable-views="['years']"
      style="height: 650px"
      :time-from="8 * 60"
      :time-to="24 * 60"
      :time-step="30"
      @cell-dblclick="handleCellClick"
      @event-click ="onEventClick"
    />

    <div v-if="showModal" class="modal">
      <div class="modal-content">
        <button v-if="eventTypeSelected" @click="goBack" class="back-button">
          ← Tagasi
        </button>
        <h2>{{ modalTitle }}</h2>
        
        <div v-if="!eventTypeSelected">
          <button @click="selectEventType('normal')">Tavasündmus</button>
          <button @click="selectEventType('training')">Treeningu alustamine</button>
        </div>

        <form v-if="eventTypeSelected" @submit.prevent="handleEventSubmit">
          <div>
            <label for="category">Kategooria:</label>
            <input
              type="text"
              id="category"
              :value="eventCategory"
              readonly
              disabled
            />
          </div>
          
          <div v-if="eventType === 'normal'">
            <label for="title">Pealkiri:</label>
              <input
                type="text"
                id="title"
                v-model="newEvent.title"
                required/>

            <label for="start">Algus:</label>
              <input
                type="datetime-local"
                id="start"
                v-model="newEvent.start"
                required/>

            <label for="end">Lõpp:</label>
              <input
                type="datetime-local"
                id="end"
                v-model="newEvent.end"
                required/>

            <label for="description">Kirjeldus:</label>
              <textarea
                id="description"
                v-model="newEvent.description"></textarea>

            <button type="submit">{{ newEvent.id ? 'Muuda' : 'Lisa' }}</button>
          </div>

          <div v-if="eventType === 'training'">
            <label for="trainingPlan">Treeningkava:</label>
            <div id="trainingPlan" class="training-plan">
              {{ planStore.selectedPlan?.title }}
            </div>
            <label for="start">Algus:</label>
              <input
                type="datetime-local"
                id="start"
                v-model="newEvent.start"
                required/>
              <button type="submit">Alusta treeningut</button>
          </div>
          <button @click="closeModal" type="button">Sulge</button>
        </form>
      </div>
    </div>
    <div v-if="showEventDetailsModal" class="modal-overlay">
      <div class="modal-content">
        <h2>{{ isEditing ? 'Muuda sündmust' : 'Sündmuse detailid' }}</h2>
        <div v-if="isEditing">
          <!-- Redigeerimise vorm -->
          <form @submit.prevent="handleEventSubmit">
            <div>
              <label for="title">Pealkiri:</label>
              <input type="text" id="title" v-model="newEvent.title" required />
            </div>
            <div>
              <label for="start">Algus:</label>
              <input type="datetime-local" id="start" 
              :value="formatForDatetimeLocal(newEvent.start)" 
              @input="newEvent.start = $event.target.value"
              required />
            </div>
            <div>
              <label for="end">Lõpp:</label>
              <input type="datetime-local" id="end" 
              :value="formatForDatetimeLocal(newEvent.end)" 
              @input="newEvent.end = $event.target.value" 
              required 
              />
            </div>
            <div>
              <label for="description">Kirjeldus:</label>
              <textarea id="description" v-model="newEvent.description"></textarea>
            </div>
            <button type="submit">Salvesta muudatused</button>
          </form>
        </div>

        <div v-else>
          
          <p><strong>Pealkiri:</strong> {{ selectedEvent.title }}</p>
          <p><strong>Algus:</strong> {{ formatDate(selectedEvent.start) }}</p>
          <p><strong>Lõpp:</strong> {{ formatDate(selectedEvent.end) }}</p>
          <p><strong>Kirjeldus:</strong> {{ selectedEvent.description || 'Puudub' }}</p>
          <button @click="closeEventDetailsModal">Sulge</button>
          <!--<button @click="editEvent">Muuda</button> -->
        </div>
    </div>
  </div>
  </div>

</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import VueCal from 'vue-cal';
import 'vue-cal/dist/vuecal.css';
import { useEventStore } from '~/stores/eventStore';
import { format } from 'date-fns';
import { et } from 'date-fns/locale';
import { useRouter } from 'vue-router';

const eventStore = useEventStore();
const planStore = usePlanStore();
const { events, loadEvents, addEvent, updateEvent, deleteEvent } = eventStore;
const router = useRouter();


const showModal = ref(false);
const selectedEvent = ref({});
const showEventDetailsModal = ref(false);
const isEditing = ref(false);
const newEvent = ref({
  title: '',
  start: '',
  end: '',
  description: '',
  trainingPlan: '',
});

const eventTypeSelected = ref(false); 
const eventType = ref(''); 
const trainingStartTime = ref(null);

const modalTitle = computed(() => {
  if (!eventTypeSelected.value) {
    return 'Vali sündmuse tüüp:';
  }
  return eventType.value === 'normal' ? 'SÜNDMUS' : 'TREENING';
});

onMounted(async () => {
  await loadEvents();
  router.push("/workout-calendar");
  console.log("On mounted check:", events);
});

const handleCellClick = (date) => { //algväärtused modalis
  showModal.value = true;
  const startDateTime = new Date(date);
  newEvent.value.start = format(startDateTime, "yyyy-MM-dd' 'HH:mm", { locale: et });
  const endDateTime = new Date(startDateTime.getTime() + 60 * 60 * 1000); //vaikimisi: event on 1h pikk
  newEvent.value.end = format(endDateTime, "yyyy-MM-dd' 'HH:mm", { locale: et });
  newEvent.value.description = ''; 
  newEvent.value.trainingPlan = '';
  eventTypeSelected.value = false;
};

const selectEventType = (type) => {
  eventTypeSelected.value = true;
  eventType.value = type;
  if (type === 'normal') {
    newEvent.value.trainingPlan = ''; // Treeningkava tühjendamine, kui valitakse tavasündmus
  }
};
const goBack = () => {
  eventTypeSelected.value = false;
};
const formatForDatetimeLocal = (date) => {
  if (!date) return '';
  const formattedDate = format(new Date(date), 'yyyy-MM-dd\'T\'HH:mm'); // Lisame 'T' kuupäeva ja aja vahele
  return formattedDate;
};
// Uue sündmuse lisamine (vorm tuleb)
 const handleEventSubmit = async () => {
  if (eventType.value === 'normal') {
    const newEventData = {
      ...newEvent.value,
      start: formatDate(newEvent.value.start),
      end: formatDate(newEvent.value.end),
      class: 'event',
      content: '⇢ Lisainfo ⇠',
    }
   if (!newEvent.value.id) {
     await addEvent(newEventData); 
   } else {
    const updatedEventData = {
      title: newEvent.value.title,
      start: newEvent.value.start,
      end: newEvent.value.end,
      description: newEvent.value.description,
};
     await updateEvent(newEvent.value.id, updatedEventData);
   }
   closeModal();
   await loadEvents(); //ToDo: kui lisad sündmuse, peaks kohe kalendris näha olema (võiks modalis muutmine olla)
                      //ToDo: kustutamine toimiks ka andmebaasis
  }
  else {
    if (eventType.value === 'training') {
      const startDateTime = new Date(newEvent.value.start); // Start on juba määratud
      const endDateTime = new Date(startDateTime.getTime() + 60 * 60 * 1000); // Lõpp 1 tund hiljem

      newEvent.value.end = format(endDateTime, "yyyy-MM-dd' 'HH:mm", { locale: et });
    }
    const newEventData = {
      ...newEvent.value,
      start: formatDate(newEvent.value.start),
      end: formatDate(newEvent.value.end),
      class: 'training',
      content: '⇢ Lisainfo ⇠',
    }
    await addEvent(newEventData);
    closeModal();
    await loadEvents();
    startTraining();
  }
   
};
const closeModal = () => {
  showModal.value = false;
  newEvent.value = { title: '', start: '', end: '', description: '', trainingPlan: '' };
  eventTypeSelected.value = false;
}

const startTraining = () => {
  trainingStartTime.value = newEvent.value.start;
  router.push({
    path: '/Workout',
    query: { startTime: trainingStartTime.value }
  });
 
  console.log(trainingStartTime);
};

const formatDate = (date) => {
  return format(new Date(date), 'yyyy-MM-dd HH:mm', { locale: et });
};


const onEventClick = (event) => {
  selectedEvent.value = event;
  showEventDetailsModal.value = true;
};

const closeEventDetailsModal = () => {
  showEventDetailsModal.value = false;
  isEditing.value = false;
};
const editEvent = () => {
  isEditing.value = true;
  newEvent.value = { ...selectedEvent.value };
  eventTypeSelected.value = true;
  eventType.value = selectedEvent.value.class === 'training' ? 'training' : 'normal';
};
const eventCategory = computed(() => {
  return eventType.value === 'training' ? 'Treeningsündmus' : 'Tavasündmus';
});


</script>
  
<style>
  @import '../designs/calendar.css';
</style>
  