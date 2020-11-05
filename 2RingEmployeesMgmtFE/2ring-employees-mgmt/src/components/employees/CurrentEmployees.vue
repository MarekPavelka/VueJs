<template>
  <div>
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
      <div id="div-header" class="row mb-3 mr-auto myAboveTable">
        <b-button
          pill
          type="button"
          variant="success"
          class="ml-auto"
          @click="navigateToCreateEmployee()"
        >
          <b-icon icon="plus" font-scale="1"></b-icon>
          Add employee
        </b-button>
      </div>
      <table class="table table-bordered table-hover shadow-sm">
        <thead class="thead-dark">
          <tr>
            <th scope="col">Name</th>
            <th scope="col">Recent Position</th>
            <th scope="col"></th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="!currentEmployees.length">
            <td colspan="4" class="text-center">
              No current employees found
            </td>
          </tr>
          <template v-else>
            <tr v-for="employee in currentEmployees" :key="employee.id">
              <td>
                {{ employee | employeeFullName }}
              </td>
              <td>
                {{
                  employee.Position.Id == 0
                    ? "N/A"
                    : employee.Position.PositionName
                }}
              </td>
              <td class="text-right">
                <!-- <router-link class="btn btn-sm btn-secondary" to=""></router-link> -->
                <b-button
                  type="button"
                  size="sm"
                  variant="outline-secondary"
                  v-b-popover.hover="{
                    variant: 'dark',
                    content: 'Edit employee',
                  }"
                  @click="navigateToEditEmployee(employee.Id)"
                >
                  <b-icon icon="tools"></b-icon>
                </b-button>
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
                  @click="
                    modalShow = !modalShow;
                    selectedEmployee = employee;
                  "
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
      id="modal-archive"
      v-model="modalShow"
      :header-bg-variant="'warning'"
      title="Archive employee"
    >
      <div class="row justify-content-center my-4">
        <p>Archive employee</p>
        <p class="mx-2 font-weight-bold font-italic">
          {{ selectedEmployee | employeeFullName }}
        </p>
        <p>before deletion?</p>
      </div>
      <template #modal-footer>
        <div class="w-100">
          <div class="col-6 offset-6">
            <div class="d-flex justify-content-end">
              <b-button
                variant="danger"
                @click="deleteEmployee(selectedEmployee)"
                >Delete</b-button
              >
              <b-button
                class="ml-2"
                variant="info"
                @click="archiveEmployee(selectedEmployee)"
                >Archive</b-button
              >
            </div>
          </div>
        </div>
      </template>
    </b-modal>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Employee from "@/models/Employee";
import Position from "@/models/Position";
import axios from "axios";
import EmployeeService from "@/services/EmployeeService";

@Component({
  components: {},
})
export default class CurrentEmployees extends Vue {
  isLoadingData: boolean = false;
  modalShow: boolean = false;
  selectedEmployee: Employee = new Employee();
  currentEmployees: Employee[] = [];
  addEmployeeRoute: any = { name: "addEmployee" };
  editEmployeeRoute: any = { name: "editEmployee" };

  created() {
    this.initializeEmployees();
  }

  async initializeEmployees(): Promise<void> {
    try {
      this.isLoadingData = true;
      const allEmployees = await EmployeeService.getAllEmployees();
      this.currentEmployees = allEmployees.filter((e) => e.IsArchived == false);
    } catch (error) {
      this.showErrorNotification(error);
    } finally {
      this.isLoadingData = false;
    }
  }

  navigateToCreateEmployee(): void {
    this.$router.push(this.addEmployeeRoute);
  }

  navigateToEditEmployee(employeeId: number): void {
    this.editEmployeeRoute.params = { id: employeeId };
    this.$router.push(this.editEmployeeRoute);
  }

  async archiveEmployee(employee: Employee): Promise<void> {
    try {
      this.isLoadingData = true;
      await EmployeeService.archiveEmployee(employee.Id);
      this.$notification.success("Emplopyee archived successfully");
      this.currentEmployees = this.currentEmployees.filter(
        (e) => e.Id != employee.Id
      );
    } catch (error) {
      this.showErrorNotification(error);
    } finally {
      this.selectedEmployee = new Employee();
      this.$root.$emit("bv::hide::modal", "modal-archive");
      this.isLoadingData = false;
    }
  }

  async deleteEmployee(employee: Employee): Promise<void> {
    try {
      this.isLoadingData = true;
      await EmployeeService.deleteEmployee(employee.Id);
      this.currentEmployees = this.currentEmployees.filter(
        (e) => e.Id != employee.Id
      );
      this.$notification.success("Employee deleted successfully");
    } catch (error) {
      if (error.response.data) {
        this.showErrorNotification(error);
      } else {
        this.$notification.error(error);
      }
    } finally {
      this.selectedEmployee = new Employee();
      this.$root.$emit("bv::hide::modal", "modal-archive");
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
