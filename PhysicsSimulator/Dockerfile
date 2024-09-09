FROM node:lts-alpine AS builder

# make the 'app' folder the current working directory
WORKDIR /app

# copy both 'package.json' and 'package-lock.json' (if available)
COPY package*.json ./

# install project dependencies
RUN npm install

# copy project files and folders to the current working directory (i.e. 'app' folder)
COPY . .

# build app for production with minification
RUN npm run build-only

# nginx state for serving content
FROM nginx:stable-alpine as production-stage

# Set working directory to nginx asset directory
WORKDIR /usr/share/nginx/html

# Remove default nginx static assets
RUN rm -rf ./*

# Copy static assets from builder stage
COPY --from=builder /app/dist .

# Copy custom Nginx configuration
COPY default.conf /etc/nginx/conf.d/default.conf

# This first changes the ownership of all files and directories in the current directory to the nginx user and group.
# Then, it sets permissions such that the nginx user has control (read, write, execute) over the files, and the
# group and others have read and execute permissions. This ensures that NGINX running inside the Docker container can
# access and serve these files appropriately.
RUN chown -R nginx:nginx . && chmod -R 755 .

EXPOSE 80
EXPOSE 443

# Containers run nginx with global directives and daemon off
ENTRYPOINT ["nginx", "-g", "daemon off;"]