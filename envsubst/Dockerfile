FROM alpine:latest
RUN apk add --update --no-cache libintl gettext

WORKDIR /workdir

COPY envsubst-file.sh /

ENTRYPOINT ["/envsubst-file.sh"]
