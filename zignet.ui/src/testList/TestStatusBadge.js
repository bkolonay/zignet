import React, { Component } from 'react';

class TestStatusBadge extends Component {

  render() {
    const badge = this.props.failingFromDate ? 
      <span className='badge badge-danger'>Down</span> :
      <span className='badge badge-success'>Up</span>;

    return (
      badge
    );
  }
}

export default TestStatusBadge;
