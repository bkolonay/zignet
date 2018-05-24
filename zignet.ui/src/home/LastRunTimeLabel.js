import React, { Component } from 'react';
import { getTimeFromNowWithSuffix } from '../common/UtcDateProvider'

class LastRunTimeLabel extends Component {

  render() {

    let label = null;
    if (this.props.grouped)
    	label = <p/>;
    else if (this.props.suiteEndTime)
    	label = <p className="text-center text-muted"><small>{getTimeFromNowWithSuffix(this.props.suiteEndTime)}</small></p>;
    else
    	label = <p className="text-center text-warning"><small>running...</small></p>;

    return (
      label
    );
  }
}

export default LastRunTimeLabel;
