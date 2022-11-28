FROM node:12-alpine as build

WORKDIR /app
COPY . /app/

RUN npm install --save --legacy-peer-deps

RUN npm run build

FROM nginx:1.15-alpine
COPY --from=build /app/dist/client /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
