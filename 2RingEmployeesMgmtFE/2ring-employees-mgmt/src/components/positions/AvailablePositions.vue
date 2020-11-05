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
          v-b-modal.modal-addPosition
          variant="success"
          class="ml-auto"
        >
          <b-icon icon="plus" font-scale="1"></b-icon>
          Add position
        </b-button>
      </div>
      <table class="table table-bordered table-hover shadow-sm">
        <thead class="thead-dark">
          <tr>
            <th scope="col">Name of position</th>
            <th scope="col">Date created</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="!allPositions.length">
            <td colspan="4" class="text-center">
              No positions found
            </td>
          </tr>
          <template v-else>
            <tr v-for="position in allPositions" :key="position.id">
              <td>{{ position.PositionName }}</td>
              <td>{{ position.LastModified | filterDate }}</td>
              <td class="text-right">
                <b-button
                  id="btn-delete"
                  type="button"
                  size="sm"
                  variant="outline-danger"
                  v-b-popover.hover="{
                    variant: 'danger',
                    content: 'Delete position',
                  }"
                  v-b-modal="'modal-deletePosition'"
                  @click="selectedPosition = position"
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
      id="modal-addPosition"
      ref="modal"
      :header-bg-variant="'light'"
      :footer-bg-variant="'light'"
      size="lg"
      title="Create new position"
      centered
      ok-title="Create"
      ok-variant="primary"
      @show="resetModal"
      @hidden="resetModal"
      @ok="handleCreatePosition($event)"
    >
      <b-form
        ref="formAddPosition"
        @submit.stop.prevent="handleSubmit()"
        novalidate
      >
        <b-form-group
          class="col-md-8 offset-md-2"
          :state="isPositionNameValid"
          label="Position name"
          label-for="name-input"
          invalid-feedback="Position name is required"
        >
          <b-form-input
            required
            :state="isPositionNameValid"
            v-model="positionToAdd.PositionName"
          />
        </b-form-group>
      </b-form>
    </b-modal>
    <b-modal
      id="modal-deletePosition"
      :header-bg-variant="'warning'"
      ok-title="Delete"
      ok-variant="danger"
      @ok="deletePosition(selectedPosition)"
    >
      <template #modal-title>
        Delete position
      </template>
      <div class="row justify-content-center my-4">
        <p>Are you sure about deleting position</p>
        <p class="ml-2 font-weight-bold font-italic">
          {{ selectedPosition.PositionName }}?
        </p>
      </div>
    </b-modal>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Position from "@/models/Position";
import PositionService from "@/services/PositionService";
import EmployeeService from "@/services/EmployeeService";

@Component
export default class AvailablePositions extends Vue {
  isLoadingData: boolean = false;
  allPositions: Position[] = [];
  selectedPosition: Position = new Position();
  positionToAdd: Position = new Position();
  isPositionNameValid: boolean | null = null;

  created() {
    this.getPositions();
  }

  async getPositions(): Promise<void> {
    try {
      this.isLoadingData = true;
      const allPositions = await PositionService.getAllPositions();
      this.allPositions = allPositions;
    } catch (error) {
      this.showErrorNotification(error);
    } finally {
      this.isLoadingData = false;
    }
  }

  async createPosition(): Promise<void> {
    if (!this.checkFormValidity()) {
      return;
    }
    try {
      this.isLoadingData = true;
      const addedPosition = await PositionService.addPosition(
        this.positionToAdd
      );
      console.log(addedPosition);
      this.allPositions.push(addedPosition);
      this.$notification.success("Position created successfully");
    } catch (error) {
      this.showErrorNotification(error);
    } finally {
      this.selectedPosition = new Position();
      this.$nextTick(() => {
        this.$bvModal.hide("modal-addPosition");
      });
      this.isLoadingData = false;
    }
  }

  async deletePosition(): Promise<void> {
    try {
      this.isLoadingData = true;
      await PositionService.deletePosition(this.selectedPosition.Id);
      this.allPositions = this.allPositions.filter(
        (p) => p.Id != this.selectedPosition.Id
      );
      this.$notification.success("Position deleted successfully");
    } catch (error) {
      this.showErrorNotification(error);
    } finally {
      this.isLoadingData = false;
    }
  }

  private handleCreatePosition(e: any) {
    e.preventDefault();
    this.createPosition();
  }

  private checkFormValidity() {
    const isValid = (<any>this).$refs.formAddPosition.checkValidity();
    this.isPositionNameValid = isValid;
    return isValid;
  }

  private resetModal() {
    this.positionToAdd.PositionName = "";
    this.isPositionNameValid = null;
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
