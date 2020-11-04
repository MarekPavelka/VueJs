import axios from "axios";
import Position from "@/models/Position";

class PositionService {
  async getAllPositions(): Promise<Position[]> {
    const res = await axios.get<Position[]>(`EmployeePosition`);
    return res.data.map((pos) => new Position(pos));
  }
  async addPosition(position: Position): Promise<Position> {
    const res = await axios.post<Position>(`EmployeePosition`, position);
    return new Position(res.data);
  }
  deletePosition(id: Number): Promise<void> {
    return axios.delete(`EmployeePosition/${id}`);
  }
}

export default new PositionService();
