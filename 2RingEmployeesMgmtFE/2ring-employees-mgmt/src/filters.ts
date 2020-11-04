import { VueConstructor } from "vue/types/umd";
import Employee from "@/models/Employee";

export default {
  install(Vue: VueConstructor) {
    Vue.filter(
      "employeeFullName",
      (employee: Employee) => `${employee.FirstName} ${employee.Surname}`
    );
    Vue.filter("filterDate", (date: Date | null) => {
      if (date == null) return "current";
      let browserDate = new Date(date);
      return browserDate.toDateString();
    });
  },
};
