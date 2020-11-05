<template>
  <div>
    <div id="div-header" class="row mb-3 myAboveTable"></div>
    <b-overlay :show="isLoadingData" rounded="sm">
      <template #overlay>
        <div class="text-center">
          <b-icon
            icon="stopwatch"
            font-scale="3"
            animation="spin"
            variant="secondary"
          ></b-icon>
          <p id="cancel-label" class="text-secondary">Please wait...</p>
        </div>
      </template>
      <table class="table table-bordered table-hover shadow-sm">
        <thead class="thead-dark">
          <tr>
            <th scope="col">Name</th>
            <th scope="col">Last Position</th>
            <th scope="col">Dismissed date</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="!previousEmployees.length">
            <td colspan="4" class="text-center">No previous employees found</td>
          </tr>
          <template v-else>
            <tr v-for="employee in previousEmployees" :key="employee.id">
              <td>
                <a
                  href=""
                  @click.prevent="navigateToEmployeeDetails(employee.Id)"
                  >{{ employee | employeeFullName }}</a
                >
              </td>
              <td>
                {{
                  employee.Position.Id == 0
                    ? "N/A"
                    : employee.Position.PositionName
                }}
              </td>
              <td>
                {{ employee.LastModified | filterDate }}
              </td>
              <td class="text-right">
                <b-button
                  id="btn-delete"
                  type="button"
                  size="sm"
                  variant="outline-danger"
                  v-b-popover.hover="{
                    variant: 'danger',
                    content: 'Delete employee',
                  }"
                  v-b-modal="'modal-delete'"
                  @click="selectedEmployee = employee"
                >
                  <b-icon icon="trash"></b-icon>
                </b-button>
              </td>
            </tr>
          </template>
        </tbody>
      </table>
    </b-overlay>
    <b-modal
      id="modal-delete"
      :header-bg-variant="'warning'"
      ok-title="Delete"
      ok-variant="danger"
      @ok="deleteEmplopyee(selectedEmployee)"
    >
      <template #modal-title>
        Delete employee
      </template>
      <div class="row justify-content-center my-4">
        <p>Are you sure about deleting employee</p>
        <p class="ml-2 font-weight-bold font-italic">
          {{ selectedEmployee | employeeFullName }}?
        </p>
      </div>
    </b-modal>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Employee from "@/models/Employee";
import EmployeeService from "@/services/EmployeeService";

@Component({})
export default class PreviousEmployees extends Vue {
  isLoadingData: boolean = false;
  previousEmployees: Array<Employee> = [];
  selectedEmployee: Employee = new Employee();
  employeeDetailsRoute: any = { name: "previousEmployeeDetail" };

  created() {
    this.initializeEmployees();
  }

  async initializeEmployees(): Promise<void> {
    try {
      this.isLoadingData = true;
      this.previousEmployees = await EmployeeService.getArchivedEmployees();
    } catch (error) {
      this.showErrorNotification(error);
    } finally {
      this.isLoadingData = false;
    }
  }

  navigateToEmployeeDetails(employeeId: Number): void {
    this.employeeDetailsRoute.params = { id: employeeId };
    this.$router.push(this.employeeDetailsRoute);
  }

  async deleteEmplopyee(employee: Employee): Promise<void> {
    try {
      this.isLoadingData = true;
      await EmployeeService.deleteEmployee(employee.Id);
      this.previousEmployees = this.previousEmployees.filter(
        (e) => e.Id != employee.Id
      );
      this.$notification.success("Employee deleted successfully");
    } catch (error) {
      this.showErrorNotification(error);
    } finally {
      this.isLoadingData = false;
    }
  }

  private showErrorNotification(error: any): void {
    if (error.response) {
      this.$notification.error(error.response.data);
    } else {
      this.$notification.error(error.toString());
    }
  }
}
</script>
<style></style>
