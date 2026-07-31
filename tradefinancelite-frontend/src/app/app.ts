import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NetworkBackground } from './components/network-background/network-background';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NetworkBackground],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('tradefinancelite-frontend');
}
