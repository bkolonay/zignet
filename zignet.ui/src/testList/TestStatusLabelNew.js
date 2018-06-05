import React, { Component } from 'react';
import { getTimeFromNow } from '../common/UtcDateProvider'

class TestStatusLabelNew extends Component {

  render() {
    const label = this.props.failingFromDate ? 
      <span className='text-danger'>{'for ' + getTimeFromNow(this.props.failingFromDate)}</span> :
      <span className='text-success'>{'for ' + getTimeFromNow(this.props.passingFromDate)}</span>;

    return (
      label
    );
  }
}

export default TestStatusLabelNew;
