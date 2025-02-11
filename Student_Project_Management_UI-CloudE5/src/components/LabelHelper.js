
export const statusLabels = {
    0: "Submitted",
    1: "Under Analysis",
    2: "Feedback Ready",
    3: "Rejected",
  };
  
export const formatDate = (dateString) => {
    if (!dateString) return "N/A";
    return new Date(dateString).toLocaleString();
  };