import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import * as THREE from 'three';

@Component({
  selector: 'app-network-background',
  standalone: true,
  imports: [],
  templateUrl: './network-background.html',
  styleUrl: './network-background.css'
})
export class NetworkBackground implements AfterViewInit, OnDestroy {
  @ViewChild('vantaRef') vantaRef!: ElementRef<HTMLDivElement>;
  private vantaEffect: any;

  async ngAfterViewInit(): Promise<void> {
    (window as any).THREE = THREE;

    const module: any = await import('vanta/dist/vanta.dots.min');
    const DOTS = module.default;

    this.vantaEffect = DOTS({
      el: this.vantaRef.nativeElement,
      THREE: THREE,
      mouseControls: true,
      touchControls: true,
      gyroControls: false,
      minHeight: 200,
      minWidth: 200,
      scale: 1.0,
      scaleMobile: 1.0,
      color: 0x696cff,
      color2: 0x696cff,
      backgroundColor: 0xf5f5f9,
      size: 3,
      spacing: 35,
      showLines: true
    });
  }

  ngOnDestroy(): void {
    if (this.vantaEffect) {
      this.vantaEffect.destroy();
    }
  }
}
