var HDQ = HDQ || {};

HDQ.namespace = function (ns_string) {
    var parts = ns_string.split('.'),
        parent = HDQ,
        i;
    if (parts[0] === "HDQ") {
        parts = parts.slice(1);
    }
    for (i = 0; i < parts.length; i += 1) {
        if (typeof parent[parts[i]] === "undefined") {
            parent[parts[i]] = {};
        }
        parent = parent[parts[i]];
    }
    return parent;
};