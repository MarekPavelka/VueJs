import axios from "axios";
import Employee from "@/models/Employee";

class EmployeeService {
  async createEmployee(employee: Employee): Promise<Employee> {
    const res = await axios.post<Employee>("Employees", employee);
    return new Employee(res.data);
  }
  updateEmployee(employee: Employee): Promise<void> {
    return axios.put(`Employees/${employee.Id}`, employee);
  }
  async getEmployee(id: Number): Promise<Employee> {
    const res = await axios.get<Employee>(`Employees/${id}`);
    return new Employee(res.data);
  }
  async getAllEmployees(): Promise<Employee[]> {
    const res = await axios.get<Employee[]>(`Employees`);
    return res.data.map((e) => new Employee(e));
  }
  async getArchivedEmployees(): Promise<Employee[]> {
    const res = await axios.get<Employee[]>(`Employees/archived`);
    return res.data.map((e) => new Employee(e));
  }

  archiveEmployee(id: Number): Promise<void> {
    return axios.put(`Employees/archive/${id}`);
  }

  deleteEmployee(id: Number): Promise<void> {
    return axios.delete(`Employees/${id}`);
  }
}
export default new EmployeeService();
