import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  private socket: WebSocket;
  private messageSubject = new Subject<any>();

  public recived: boolean = true;
  public messages$: Observable<any> = this.messageSubject.asObservable();

  constructor() {
    // Connect to the WebSocket server

    this.socket = new WebSocket('ws://localhost:8181');

    this.socket.onopen = () => {
      console.log("recieved");

    }

    this.socket.onclose = () => {
      console.log("closed");

    }

    this.socket.onmessage = (event) => {
      const data = event.data;
      console.log(data);
      this.messageSubject.next(data);
    };

    this.socket.onerror = (event) => {
      console.log(event);
    }
  }
}
