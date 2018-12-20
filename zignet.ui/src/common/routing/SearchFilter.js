import { parseUrlParams, stringifyUrlParams } from './UrlParameterParser.js';

function getFilter(search) {
	let urlParams = parseUrlParams(search);

	let filter = {
		debug: false,
		showLoopNet: false,
		showLmMobile: false,
		showCityFeet: false,
		showShowcase: false
	};

	if (urlParams.debug === 'true')
		filter.debug = true;
	if (urlParams.applications && urlParams.applications.includes('LoopNet'))
		filter.showLoopNet = true;
	if (urlParams.applications && urlParams.applications.includes('Listing Manager Mobile'))
		filter.showLmMobile = true;
	if (urlParams.applications && urlParams.applications.includes('CityFeet'))
		filter.showCityFeet = true;
	if (urlParams.applications && urlParams.applications.includes('Showcase'))
		filter.showShowcase = true;	

	return filter;
}

function getUrlParams(filter) {
	let urlParams = {
	  debug: false,
	  applications: []
	};

	if (filter.debug)
		urlParams.debug = true;
	if (filter.showLoopNet)
	  urlParams.applications.push('LoopNet');
	if (filter.showLmMobile)
	  urlParams.applications.push('Listing Manager Mobile');
	if (filter.showCityFeet)
	  urlParams.applications.push('CityFeet');
	if (filter.showShowcase)
	  urlParams.applications.push('Showcase');	

	return stringifyUrlParams(urlParams);
}

export { getFilter, getUrlParams }