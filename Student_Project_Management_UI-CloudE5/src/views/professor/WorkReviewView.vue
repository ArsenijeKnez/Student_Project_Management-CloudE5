<script setup> 
import { ref, onMounted } from "vue";
import submissionService from "@/services/submissionService";
import DisplayWork from "@/components/DisplayWork.vue";
import { useRoute } from "vue-router";
import UpdateFeedback from "@/components/professor/UpdateFeedback.vue"

const message = ref("");
const studentWorks = ref(null);
const route = useRoute();

const fetchStudentWork = async () => {
  const studentWorkId = route.query.id; 

  if (!studentWorkId) {
    message.value = "Student Work ID is missing.";
    return;
  }

  try {
    const response = await submissionService.getWorksForStudent(studentWorkId);

    if (response.status === 200) {
      studentWorks.value = response.data;
    } else {
      message.value = "Failed to fetch student work.";
    }
  } catch (error) {
    message.value = "Failed to retrieve student work.";
  }
};

onMounted(fetchStudentWork);
</script>

<template>
  <div >
    <h2 class="title">{{ route.query.username }}'s submissions</h2> 

    <p v-if="message" class="error">{{ message }}</p>

    <div v-if="studentWorks && !message && studentWorks.length">
      <ul class="work-list">
        <li v-for="(work, index) in studentWorks" :key="index" class="work-item">
          <DisplayWork :studentWork="work" /> 
          <UpdateFeedback v-if="work.status != 0 && work.feedback" :feedback="work.feedback" :studentWorkId="work.id"/>
        </li>
      </ul>
    </div>
    
    <div v-else class="loading">Loading...</div>
  </div>
</template>

<style scoped>

.title {
  text-align: center;
  color: #ffffff;
  margin-bottom: 12px;
}

.error {
  color: red;
  text-align: center;
  font-weight: bold;
}

.work-list {
  list-style-type: none;
  padding: 0;
}

.work-item {
  background: #0d0d0d;
  padding: 12px;
  margin-bottom: 12px;
  border-radius: 6px;
  box-shadow: rgba(102, 189, 115, 0.25) 0px 50px 100px -20px, rgba(255, 255, 255, 0.3) 0px 30px 60px -30px, rgba(165, 192, 165, 0.35) 0px -2px 6px 0px inset;
}

.loading {
  text-align: center;
  font-style: italic;
}
</style>
