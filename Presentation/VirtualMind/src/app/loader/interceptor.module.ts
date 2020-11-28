import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoaderInterceptor } from './loader.interceptor';
@NgModule({
 providers: [
  LoaderInterceptor,
  {
    provide: HTTP_INTERCEPTORS,
    useClass: LoaderInterceptor,
    multi: true,
  },
 ],
})
export class InterceptorModule {}
