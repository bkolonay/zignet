import moment from 'moment';

function getTimeFromNow(utcDateTime) {
	let localMoment = moment.utc(utcDateTime);
	return localMoment.fromNow(true);
}

function getTimeFromNowWithSuffix(utcDateTime) {
	let localMoment = moment.utc(utcDateTime);
	return localMoment.fromNow();
}

export { getTimeFromNow, getTimeFromNowWithSuffix }