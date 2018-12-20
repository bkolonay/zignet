import { parse, stringify } from 'query-string';

function parseUrlParams(search) {
	return parse(search);
}

function stringifyUrlParams(params) {
	return stringify(params);
}

export { parseUrlParams, stringifyUrlParams }