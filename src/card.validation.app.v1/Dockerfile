# Stage 1: Build React App
FROM node:18 AS build

WORKDIR /app
COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# Stage 2: Serve with Nginx
FROM nginx:alpine

# Copy custom nginx.conf
COPY nginx.conf /etc/nginx/nginx.conf

# Copy built React app to nginx public dir
COPY --from=build /app/build /usr/share/nginx/html

EXPOSE 3105

CMD ["nginx", "-g", "daemon off;"]
